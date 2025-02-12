using FakeFlightBookingAPI.Data;
using FakeFlightBookingAPI.Models;
using FakeFlightBookingAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SharedModels;

namespace FakeFlightBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly IEmailService _emailService;
        public UsersController(ApplicationDbContext context, IMemoryCache cache, IEmailService emailService)
        {
            _context = context;
            _cache = cache;
            _emailService = emailService;
        }

        private async Task SendEmail(string subject, string message, string email)
        {
            try
            {
                await _emailService.SendEmailAsync(email, subject, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        // Helper method to check if an email exists
        private async Task<Customer> CheckIfEmailExists(string email)
        {
            return await _context.CustomerUsers.FirstOrDefaultAsync(c => c.Email == email);
        }

        [HttpPost("initiate-registration")]
        public async Task<IActionResult> InitiateRegistration([FromBody] Customer customer)
        {
            try
            {
                var existingEmail = await CheckIfEmailExists(customer.Email);
                if (existingEmail != null)
                {
                    return BadRequest("Email is already registered.");
                }

                var verificationCode = new Random().Next(100000, 999999).ToString();
                _cache.Set("pendingCustomer", customer);
                _cache.Set("verificationCode", verificationCode, TimeSpan.FromMinutes(10));

                string subject = "Email Verification";
                string message = $"Your verification code is: **{verificationCode}**\n\nThank you!";

                await SendEmail(subject, message, customer.Email);
                return Ok("Verification email sent.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("verify-email-and-create-account")]
        public async Task<IActionResult> VerifyEmailAndCreateAccount([FromBody] Models.EmailVerification emailVerification)
        {
            // Retrieve pending customer and verification code from cache
            if (!_cache.TryGetValue("pendingCustomer", out Customer pendingCustomer) ||
                !_cache.TryGetValue("verificationCode", out string storedVerificationCode))
            {
                return BadRequest("Invalid request. Please try registering again.");
            }

            // Check if the email matches
            if (pendingCustomer.Email != emailVerification.Email)
            {
                return BadRequest("Email mismatch.");
            }

            // Verify the code
            if (storedVerificationCode == emailVerification.Code)
            {
                // Hash the password before creating the account
                (string hashedPassword, string salt) = PasswordHelper.HashPassword(pendingCustomer.Password);

                // Set the hashed password and salt
                pendingCustomer.Password = hashedPassword;
                pendingCustomer.Salt = salt; // Ensure salt is set

                // Verification successful, now create the customer in the database
                _context.CustomerUsers.Add(pendingCustomer);
                await _context.SaveChangesAsync();

                _cache.Remove("pendingCustomer");
                _cache.Remove("verificationCode");

                return Ok("Email successfully verified and account created.");
            }

            return BadRequest("Invalid verification code.");
        }

        [HttpPost("Send-Reset-Password-Email")]
        public async Task<IActionResult> InitiatePasswordReset([FromBody] SharedModels.ForgotPassword_Poco ForgotPassword_Poco)
        {
            var userEmail = await CheckIfEmailExists(ForgotPassword_Poco.Email);

            if (userEmail == null || string.IsNullOrEmpty(ForgotPassword_Poco.Email))
            {
                return BadRequest("Invalid email.");
            }

            var verificationCode = new Random().Next(100000, 999999).ToString();
            _cache.Set("verificationCode", verificationCode, TimeSpan.FromMinutes(10));
            _cache.Set("existingCustomerEmail", ForgotPassword_Poco.Email);

            string subject = "Reset Password";
            string message = $"Here is your verification code to reset your email the code is: **{verificationCode}**\n\nIf you did not want to reset your password please ignore this email";

            await SendEmail(subject, message, ForgotPassword_Poco.Email);
            return Ok("If this email is registered, you will receive a password reset link.");
        }

        [HttpPost("verify-code-and-change-password")]
        public async Task<IActionResult> VerifyCodeChangePassword([FromBody] SharedModels.ForgotPassword_Poco ForgotPassword_Poco)
        {
            if (string.IsNullOrEmpty(ForgotPassword_Poco.code) || string.IsNullOrEmpty(ForgotPassword_Poco.newPassword))
            {
                return BadRequest("All fields are required.");
            }

            // Retrieve pending customer and verification code from cache
            if (!_cache.TryGetValue("verificationCode", out string storedVerificationCode) ||
                 !_cache.TryGetValue("existingCustomerEmail", out string email))
            {
                return BadRequest("Invalid or expired verification code.");
            }

            // Verify the code and email
            if (ForgotPassword_Poco.code != storedVerificationCode)
            {
                return BadRequest("Invalid verification code.");
            }

            var existingCustomer = await _context.CustomerUsers.FirstOrDefaultAsync(c => c.Email == email);
            if (existingCustomer == null)
            {
                return BadRequest("User not found.");
            }

            (string hashedPassword, string salt) = PasswordHelper.HashPassword(ForgotPassword_Poco.newPassword);

            existingCustomer.Password = hashedPassword;
            existingCustomer.Salt = salt;

            _context.CustomerUsers.Update(existingCustomer);
            await _context.SaveChangesAsync();

            // Clean up cache
            _cache.Remove("verificationCode");
            _cache.Remove("existingCustomerEmail");

            return Ok("Password has been successfully reset.");
        }

        [HttpPost("CustomerLogin")]
        public async Task<IActionResult> CustomerLogin([FromBody] LoginRequest loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.UserName) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Username and password are required.");
            }

            var normalisedUserName = loginRequest.UserName.ToLower();

            // Log incoming username
            Console.WriteLine($"Normalized Username: {normalisedUserName}");

            var existingUser = await _context.CustomerUsers
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == normalisedUserName);

            if (existingUser == null)
            {
                Console.WriteLine("User not found.");
                return Unauthorized("Invalid username or password.");
            }

            Console.WriteLine($"Stored Hash: {existingUser.Password}");
            Console.WriteLine($"Stored Salt: {existingUser.Salt}");
            Console.WriteLine($"Input Password: {loginRequest.Password}");


            bool isPasswordValid = PasswordHelper.VerifyPassword
            (
                loginRequest.Password,
                existingUser.Password,
                existingUser.Salt
            );

            Console.WriteLine($"Password Valid: {isPasswordValid}");

            if (!isPasswordValid)
            {
                return Unauthorized("Invalid username or password.");
            }

            // Create a CustomerDTO object to send back to the client
            var customerDto = new CustomerDTO
            {
                Id = existingUser.Id,
                UserName = existingUser.UserName,
                FirstName = existingUser.FirstName,
                LastName = existingUser.LastName,
                Email = existingUser.Email
            };

            // Return the customer details in the response body
            return Ok(customerDto); // This will include the customer details
        }


        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile(int userId)
        {
            var user = await _context.CustomerUsers.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            return Ok(user);
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] Customer updatedCustomer)
        {
            var existingCustomer = await _context.CustomerUsers.FindAsync(updatedCustomer.Id);
            if (existingCustomer == null)
            {
                return NotFound("User not found.");
            }

            existingCustomer.FirstName = updatedCustomer.FirstName;
            existingCustomer.LastName = updatedCustomer.LastName;
            existingCustomer.Email = updatedCustomer.Email;
            existingCustomer.UserName = updatedCustomer.UserName;

            await _context.SaveChangesAsync();
            return Ok("Profile updated successfully.");
        }

        [HttpDelete("delete-account")]
        public async Task<IActionResult> DeleteAccount(int userId)
        {
            var user = await _context.CustomerUsers.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            _context.CustomerUsers.Remove(user);
            await _context.SaveChangesAsync();
            return Ok("Account deleted successfully.");
        }
    }
}

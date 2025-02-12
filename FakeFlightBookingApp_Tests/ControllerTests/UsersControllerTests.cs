using Xunit;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Mvc;
using FakeFlightBookingAPI.Controllers;
using FakeFlightBookingAPI.Models;
using FakeFlightBookingAPI.Services;
using FakeFlightBookingAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SharedModels;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using SendGrid.Helpers.Mail;

namespace FakeFlightBookingApp_Tests.ControllerTests
{
    public class UsersControllerTests : IDisposable
    {
        private readonly UsersController _controller;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMemoryCache _memoryCache;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Customer _testCustomer;

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted(); // Deletes the in-memory database
            _dbContext.Dispose(); // Disposes the DbContext
        }

        public UsersControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                       .UseInMemoryDatabase("TestDatabase")
                       .Options;
            _dbContext = new ApplicationDbContext(options);
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _mockEmailService = new Mock<IEmailService>();

            _controller = new UsersController(_dbContext, _memoryCache, _mockEmailService.Object);

            _testCustomer = new Customer("John", "Doe", "JohnDoe", "johnDoe@TestEmail.com", "password123", "0123456789");
        }

        [Fact]
        public async Task InitiateRegistration_EmailAlreadyExists_ReturnsBadRequest()
        {
            // Arrange
            _memoryCache.Remove("pendingCustomer");
            _memoryCache.Remove("verificationCode");

            var existingCustomer = new Customer("John", "Doe", "JohnDoe", "johnDoe@TestEmail.com", "password123", "0123456789");
            _dbContext.CustomerUsers.Add(existingCustomer);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _controller.InitiateRegistration(_testCustomer);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Email is already registered.", badRequestResult.Value);
        }

        [Fact]
        public async Task InitiateRegistration_ValidCustomer_SetsCacheAndSendsEmail()
        {
            //Arrange
            _memoryCache.Remove("pendingCustomer");
            _memoryCache.Remove("verificationCode");


            // Act
            var result = await _controller.InitiateRegistration(_testCustomer);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Verification email sent.", okResult.Value);

            // Verify that the customer is cached
            Assert.True(_memoryCache.TryGetValue("pendingCustomer", out var cachedCustomer));
            Assert.Equal(_testCustomer, cachedCustomer);

            _mockEmailService.Verify(
                m => m.SendEmailAsync(_testCustomer.Email, "Email Verification", It.IsAny<string>()),
                Times.Once
            );
        }

        [Fact]
        public async Task VerifyEmailAndCreateAccount_ValidCode_CreatesCustomerAccount()
        {
            // Arrange
            _memoryCache.Remove("pendingCustomer");
            _memoryCache.Remove("verificationCode");

            string verificationCode = "123456";

            var emailVerification = new EmailVerification
            {
                Email = _testCustomer.Email,
                Code = verificationCode
            };

            // Add test data to the memory cache
            _memoryCache.Set("pendingCustomer", _testCustomer);
            _memoryCache.Set("verificationCode", verificationCode);

            // Act: Call the API method
            var result = await _controller.VerifyEmailAndCreateAccount(emailVerification);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Email successfully verified and account created.", okResult.Value);

            // Verify that the customer is added to the database
            var createdCustomer = await _dbContext.CustomerUsers
                    .FirstOrDefaultAsync(c => c.Email == _testCustomer.Email); // Use query to match by email
            Assert.NotNull(createdCustomer);
            Assert.Equal(_testCustomer.Email, createdCustomer.Email);
        }

        [Fact]
        public async Task InitiatePasswordReset_CustomerExists_SendEmailAndStoreCache()
        {
            //Arrange
            _memoryCache.Remove("pendingCustomer");
            _memoryCache.Remove("verificationCode");

            var existingCustomer = _testCustomer;

            var forgotPasswordPoco = new ForgotPassword_Poco
            {
                Email = _testCustomer.Email,
                code = null,
                newPassword = null
            };

            _dbContext.CustomerUsers.Add(existingCustomer);
            await _dbContext.SaveChangesAsync();

            //Act
            var result = await _controller.InitiatePasswordReset(forgotPasswordPoco);


            //Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().Be("If this email is registered, you will receive a password reset link.");

            // Verify the email was sent
            _mockEmailService.Verify
            (
                m => m.SendEmailAsync(existingCustomer.Email, "Reset Password", It.IsAny<string>()),
                Times.Once
            );

            // Verify cache contains the verification code and email
            _memoryCache.TryGetValue("verificationCode", out string verificationCode).Should().BeTrue();
            verificationCode.Should().NotBeNullOrWhiteSpace();

            _memoryCache.TryGetValue("existingCustomerEmail", out string cachedEmail).Should().BeTrue();
            cachedEmail.Should().Be(existingCustomer.Email);

        }

        [Fact]
        public async Task VerifyCodeChangePassword_ValidCode_UpdatePasswordDeleteCache()
        {
            //Arrange
            var existingCustomer = _testCustomer;

            var forgotPasswordPoco = new ForgotPassword_Poco
            {
                Email = _testCustomer.Email,
                code = "123456",
                newPassword = "newpassword123"
            };

            _dbContext.CustomerUsers.Add(existingCustomer);
            await _dbContext.SaveChangesAsync();

            _memoryCache.Set("verificationCode", forgotPasswordPoco.code);
            _memoryCache.Set("existingCustomerEmail", _testCustomer.Email);

            //Act
            var result = await _controller.VerifyCodeChangePassword(forgotPasswordPoco);
            //Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().Be("Password has been successfully reset.");

            // Verify cache is cleared
            _memoryCache.TryGetValue("verificationCode", out string verificationCode).Should().BeFalse();

            _memoryCache.TryGetValue("existingCustomerEmail", out string cachedEmail).Should().BeFalse();
            var updatedCustomer = await _dbContext.CustomerUsers
                                             .FirstOrDefaultAsync(c => c.Email == _testCustomer.Email);

            updatedCustomer.Should().NotBeNull();
            PasswordHelper.VerifyPassword(forgotPasswordPoco.newPassword, updatedCustomer.Password, updatedCustomer.Salt).Should().BeTrue();
        }

        [Fact]
        public async Task CustomerLogin_ValidUser_ReturnsOk()
        {
            //Arrange
            (string hashedPassword, string salt) = PasswordHelper.HashPassword(_testCustomer.Password);

            _testCustomer.Password = hashedPassword;
            _testCustomer.Salt = salt;

            var existingCustomer = _testCustomer;


            _dbContext.CustomerUsers.Add(existingCustomer);
            await _dbContext.SaveChangesAsync();

            string UserEnteredPassword = "password123";
            string UserEnteredUserName = "JohnDoe";
            string normalisedUserName = UserEnteredUserName.ToLower();

            var loginRequest = new LoginRequest
            {
                UserName = UserEnteredUserName,
                Password = UserEnteredPassword
            };


            var customerDTO = new CustomerDTO
            {
                Id = existingCustomer.Id,
                UserName = existingCustomer.UserName,
                FirstName = existingCustomer.FirstName,
                LastName = existingCustomer.LastName,
                Email = existingCustomer.Email,

            };

            //Act
            var result = await _controller.CustomerLogin(loginRequest);

            // Debug: Log the result to understand if it's returning correctly
            Console.WriteLine($"Login result: {result}");

            // Assert
            var unauthorizedResult = result as UnauthorizedObjectResult;
            if (unauthorizedResult != null)
            {
                // If Unauthorized, check if the reason is password mismatch or user not found
                Console.WriteLine("Unauthorized: " + unauthorizedResult.Value);
            }

            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult); // Ensure we have a result
            Assert.IsType<CustomerDTO>(okResult.Value); // Verify the result type

            var returnedCustomer = okResult.Value as CustomerDTO;
            Assert.Equal(existingCustomer.UserName, returnedCustomer.UserName);
            Assert.Equal(existingCustomer.Email, returnedCustomer.Email);

            var storedUser = await _dbContext.CustomerUsers
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == normalisedUserName);

            // Ensure password validation works correctly
            Assert.NotNull(storedUser); // Check if the user exists in the database
            PasswordHelper.VerifyPassword(loginRequest.Password, storedUser.Password, storedUser.Salt).Should().BeTrue();
        }
    }
}

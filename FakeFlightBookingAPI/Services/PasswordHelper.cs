using System.Security.Cryptography;

namespace FakeFlightBookingAPI.Services
{
    public static class PasswordHelper
    {
        public static (string hashedPassword, string salt) HashPassword(string password)
        {
            // Generate a salt
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] saltBytes = new byte[16]; // 128 bits
                rng.GetBytes(saltBytes);
                string salt = Convert.ToBase64String(saltBytes);

                // Hash the password
                using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000)) // 10000 iterations
                {
                    byte[] hash = pbkdf2.GetBytes(20); // 160 bits
                    string hashedPassword = Convert.ToBase64String(hash);

                    // Return both the hashed password and the salt
                    return (hashedPassword, salt);
                }
            }
        }

        public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            // Convert salt back to byte array
            byte[] saltBytes = Convert.FromBase64String(storedSalt);

            // Hash the entered password with the same salt
            using (var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, saltBytes, 10000))
            {
                byte[] hash = pbkdf2.GetBytes(20); // 160 bits
                string hashedEnteredPassword = Convert.ToBase64String(hash);

                Console.WriteLine($"Generated Hash for Input Password: {hashedEnteredPassword}");

                // Compare the hashed entered password with the stored hash
                return hashedEnteredPassword == storedHash;
            }
        }
    }
}

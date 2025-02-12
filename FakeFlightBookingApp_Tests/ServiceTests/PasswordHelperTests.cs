using FakeFlightBookingAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeFlightBookingApp_Tests.ServiceTests
{
    public class PasswordHelperTests
    {
        [Fact]
        public void HashPassword_ReturnsHashedPasswordAndSalt()
        {
            // Arrange
            string password = "password123";

            // Act
            var result = PasswordHelper.HashPassword(password);

            // Assert
            Assert.NotNull(result.hashedPassword);
            Assert.NotNull(result.salt);
            Assert.NotEmpty(result.hashedPassword);
            Assert.NotEmpty(result.salt);
        }

        [Fact]
        public void VerifyPassword_CorrectPassword_ReturnsTrue()
        {
            // Arrange
            string password = "password123";
            var result = PasswordHelper.HashPassword(password);
            string hashedPassword = result.hashedPassword;
            string salt = result.salt;

            // Act
            bool isValid = PasswordHelper.VerifyPassword(password, hashedPassword, salt);

            // Assert
            Assert.True(isValid);
        }
    }
}

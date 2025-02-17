using FakeFlightBookingApp.Model;
using FakeFlightBookingApp.ViewModel;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Xunit;

namespace FakeFlightBookingApp_Tests.ViewModelTests
{
    public class CreateAccountViewModelTests
    {
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _httpClient;
        private CreateAccountViewModel _viewModel;
        private Mock<INavigationService> _mockNavigationService;

        public CreateAccountViewModelTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:7186/api/users/")
            };

            if (Application.Current == null)
            {
                try
                {
                    new Application();
                }
                catch (InvalidOperationException)
                {
                    // Application already exists, ignore
                }
            }

            Application.Current.Properties["UserId"] = null;
            Application.Current.Properties["UserName"] = null;
            Application.Current.Properties["FirstName"] = null;
            Application.Current.Properties["LastName"] = null;
            Application.Current.Properties["Email"] = null;


            _mockNavigationService = new Mock<INavigationService>();
            _viewModel = new CreateAccountViewModel(_httpClient, _mockNavigationService.Object);


        }

        [Fact]
        public void Should_Set_StatusMessage_When_Passwords_Do_Not_Match()
        {
            // Arrange
            _viewModel.FirstName = "John";
            _viewModel.LastName = "Doe";
            _viewModel.UserName = "johndoe";
            _viewModel.Email = "johndoe@example.com";
            _viewModel.Password = "Password123";
            _viewModel.ConfirmPassword = "Password124"; // Mismatched password

            // Act
            _viewModel.CreateAccountCommand.Execute(null);

            // Assert
            Assert.Equal("passwords do not match.", _viewModel.StatusMessage);
        }

        [Fact]
        public async Task Should_Set_StatusMessage_When_CreateAccount_Succeeds()
        {
            // Arrange
            _viewModel.FirstName = "John";
            _viewModel.LastName = "Doe";
            _viewModel.UserName = "johndoe";
            _viewModel.Email = "johndoe@example.com";
            _viewModel.Password = "Password123";
            _viewModel.ConfirmPassword = "Password123"; // Matching passwords

            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"message\": \"Verification email sent. Please check your email.\"}")
            };
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            await _viewModel.CreateAccountAsync();

            // Assert
            Assert.Equal("Verification email sent. Please check your email.", _viewModel.StatusMessage);
            Assert.Equal(Visibility.Visible, _viewModel.IsVerificationVisible);
        }

        [Fact]
        public async Task Should_Set_StatusMessage_When_CreateAccount_Fails()
        {
            // Arrange
            _viewModel.FirstName = "John";
            _viewModel.LastName = "Doe";
            _viewModel.UserName = "johndoe";
            _viewModel.Email = "johndoe@example.com";
            _viewModel.Password = "Password123";
            _viewModel.ConfirmPassword = "Password123"; // Matching passwords

            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("{\"error\": \"Error creating account.\"}")
            };
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            await _viewModel.CreateAccountAsync();

            // Assert
            Assert.Equal("Error creating account.", _viewModel.StatusMessage);
        }

        [Fact]
        public async Task Should_Set_StatusMessage_When_VerifyEmail_Succeeds()
        {
            // Arrange
            _viewModel.Email = "johndoe@example.com";
            _viewModel.VerificationCode = "123456";
            _viewModel.Password = "Password123";
            _viewModel.UserName = "johndoe";

            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"message\": \"Email successfully verified, and account created. Taking you to homepage\"}")
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(r => r != null && r.RequestUri.ToString().Contains("verify-email-and-create-account")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var loginResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"Id\": 1, \"UserName\": \"johndoe\", \"FirstName\": \"John\", \"LastName\": \"Doe\", \"Email\": \"johndoe@example.com\"}")
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(r => r != null && r.RequestUri.ToString().Contains("CustomerLogin")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(loginResponseMessage);

            // Act
            await _viewModel.VerifyEmailAsync();

            // Assert
            Assert.Equal("Email successfully verified, and account created. Taking you to homepage", _viewModel.StatusMessage);
            Assert.Equal(Application.Current.Properties["UserName"], "johndoe");
            Assert.Equal(Application.Current.Properties["FirstName"], "John");

            _mockNavigationService.Verify(service => service.NavigateToMainPage(), Times.Once);



        }

        [Fact]
        public async Task Should_Set_StatusMessage_When_VerifyEmail_Fails()
        {
            // Arrange
            _viewModel.Email = "johndoe@example.com";
            _viewModel.VerificationCode = "123456";
            _viewModel.Password = "Password123";
            _viewModel.UserName = "johndoe";

            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("{\"error\": \"Invalid verification code.\"}")
            };
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(r => r != null && r.RequestUri.ToString().Contains("verify-email-and-create-account")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            await _viewModel.VerifyEmailAsync();

            // Assert
            Assert.Equal("Invalid verification code.", _viewModel.StatusMessage);

        }
    }
}

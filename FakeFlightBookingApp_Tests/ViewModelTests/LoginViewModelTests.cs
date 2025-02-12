using FakeFlightBookingApp.ViewModel;
using Moq;
using Moq.Protected;
using SharedModels;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Xunit;

namespace FakeFlightBookingApp_Tests.ViewModelTests
{
    public class LoginViewModelTests
    {
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _httpClient;
        private LoginViewModel _loginViewModel;

        public LoginViewModelTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:7186/api/users/")
            };

            _loginViewModel = new LoginViewModel(_httpClient);
        }

        [Fact]
        public async Task ExecuteSignInCommand_ShouldShowSuccessMessage_WhenLoginIsSuccessful()
        {
            // Arrange
            if (Application.Current == null)
            {
                new Application(); 
            }

            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"Id\":1,\"UserName\":\"johndoe\",\"FirstName\":\"John\",\"LastName\":\"Doe\",\"Email\":\"john.doe@example.com\"}")
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse);

            _loginViewModel.UserName = "johndoe";
            _loginViewModel.Password = "password123";

            // Act
            await _loginViewModel.LoginUser(new FakeFlightBookingAPI.Models.LoginRequest
            {
                UserName = _loginViewModel.UserName,
                Password = _loginViewModel.Password
            });

            // Assert
            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri.AbsoluteUri.EndsWith("CustomerLogin")),
                ItExpr.IsAny<CancellationToken>()
            );

            Assert.Equal(1, Application.Current.Properties["UserId"]);
            Assert.Equal("johndoe", Application.Current.Properties["UserName"]);
            Assert.Equal("John", Application.Current.Properties["FirstName"]);
            Assert.Equal("Doe", Application.Current.Properties["LastName"]);
            Assert.Equal("john.doe@example.com", Application.Current.Properties["Email"]);
        }


    }
}

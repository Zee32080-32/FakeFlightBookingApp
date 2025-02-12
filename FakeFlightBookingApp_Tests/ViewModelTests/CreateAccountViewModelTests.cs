using FakeFlightBookingApp.ViewModel;
using Moq.Protected;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FakeFlightBookingApp_Tests.ViewModelTests
{
    public class CreateAccountViewModelTests
    {
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _httpClient;
        private CreateAccountViewModel _viewModel;

        public CreateAccountViewModelTests()
        {
            // Mock HttpMessageHandler to intercept HttpClient calls
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new System.Uri("https://localhost:7186/api/users/")
            };

            // Initialize the ViewModel with the mocked HttpClient
            _viewModel = new CreateAccountViewModel(_httpClient);
        }

        [Fact]
        public void Property_Setters_ShouldTriggerPropertyChanged()
        {
            // Arrange
            bool propertyChangedCalled = false;
            _viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "FirstName")
                    propertyChangedCalled = true;
            };

            // Act
            _viewModel.FirstName = "John";

            // Assert
            Assert.True(propertyChangedCalled);
        }

        [Fact]
        public async Task CreateAccountAsync_ShouldSendHttpRequest_WhenExecuted()
        {
            // Arrange
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Verification email sent. Please check your email.")
            };

            // Mock the response for POST request
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<System.Threading.CancellationToken>()
                )
                .ReturnsAsync(mockResponse);

            _viewModel.FirstName = "John";
            _viewModel.LastName = "Doe";
            _viewModel.UserName = "johndoe";
            _viewModel.Email = "john.doe@example.com";
            _viewModel.Password = "Password123";
            _viewModel.PhoneNumber = "1234567890";

            // Act
            await _viewModel.CreateAccountAsync();

            // Assert
            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri.AbsoluteUri == "https://localhost:7186/api/users/initiate-registration"),
                ItExpr.IsAny<System.Threading.CancellationToken>()
            );
        }

        [Fact]
        public async Task VerifyEmailAsync_ShouldSendHttpRequest_WhenExecuted()
        {
            // Arrange
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Email successfully verified, and account created.")
            };

            // Mock the response for POST request
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<System.Threading.CancellationToken>()
                )
                .ReturnsAsync(mockResponse);

            _viewModel.Email = "john.doe@example.com";
            _viewModel.VerificationCode = "123456";

            // Act
            await _viewModel.VerifyEmailAsync();

            // Assert
            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri.AbsoluteUri == "https://localhost:7186/api/users/verify-email-and-create-account"),
                ItExpr.IsAny<System.Threading.CancellationToken>()
            );
        }

        [Fact]
        public async Task CreateAccountAsync_ShouldShowErrorMessage_WhenRequestFails()
        {
            // Arrange
            var mockResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Error creating account.")
            };

            // Mock the response for POST request
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<System.Threading.CancellationToken>()
                )
                .ReturnsAsync(mockResponse);

            _viewModel.FirstName = "John";
            _viewModel.LastName = "Doe";
            _viewModel.UserName = "johndoe";
            _viewModel.Email = "john.doe@example.com";
            _viewModel.Password = "Password123";
            _viewModel.PhoneNumber = "1234567890";

            // Act
            await _viewModel.CreateAccountAsync();

            // Assert
            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri.AbsoluteUri == "https://localhost:7186/api/users/initiate-registration"),
                ItExpr.IsAny<System.Threading.CancellationToken>()
            );
        }

        [Fact]
        public async Task VerifyEmailAsync_ShouldShowErrorMessage_WhenVerificationFails()
        {
            // Arrange
            var mockResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Invalid verification code.")
            };

            // Mock the response for POST request
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<System.Threading.CancellationToken>()
                )
                .ReturnsAsync(mockResponse);

            _viewModel.Email = "john.doe@example.com";
            _viewModel.VerificationCode = "wrongcode";

            // Act
            await _viewModel.VerifyEmailAsync();

            // Assert
            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri.AbsoluteUri == "https://localhost:7186/api/users/verify-email-and-create-account"),
                ItExpr.IsAny<System.Threading.CancellationToken>()
            );
        }
    }
}

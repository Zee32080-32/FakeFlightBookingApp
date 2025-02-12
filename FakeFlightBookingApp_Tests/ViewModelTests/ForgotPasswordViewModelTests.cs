using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using FakeFlightBookingApp.ViewModel;
using System.Net;
using System.Windows;
using Moq.Protected;
using Stripe;

namespace FakeFlightBookingApp_Tests.ViewModelTests
{
    public class ForgotPasswordViewModelTests
    {
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _httpClient;
        private ForgotPasswordViewModel _viewModel;

        public ForgotPasswordViewModelTests()
        {
            // Mock HttpMessageHandler to intercept HttpClient calls
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new System.Uri("https://localhost:7186/api/users/")
            };

            // Initialize the ViewModel with the mocked HttpClient
            _viewModel = new ForgotPasswordViewModel(_httpClient);
        }


        [Fact]
        public async Task SendResetCodeAsync_ShouldCallPostAndReturnMessage_WhenEmailIsValid()
        {
            // Arrange: Mock the SendAsync method to return a successful response
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK);

            // Mock the response for POST request
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>
                    (
                        "SendAsync",ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && 
                        req.RequestUri.AbsoluteUri == "https://localhost:7186/api/users/Send-Reset-Password-Email"),
                        ItExpr.IsAny<System.Threading.CancellationToken>()
                    )
                    .ReturnsAsync(mockResponse);

            // Set the email
            _viewModel.Email = "testuser@example.com";

            // Act: Call the SendResetCodeAsync method
            await _viewModel.SendResetCodeAsync();

            // Assert: Check if the ShowMessage event was triggered
            Assert.Equal(Visibility.Visible, _viewModel.IsCodeInputVisible);
            Assert.Equal(Visibility.Visible, _viewModel.IsPasswordChangeVisible);

            // Verify that SendAsync was called once with the correct parameters
            _mockHttpMessageHandler.Protected().Verify
                (
                    "SendAsync",
                    Times.Once(),
                    ItExpr.Is<HttpRequestMessage>
                        (
                            req =>req.Method == HttpMethod.Post &&
                            req.RequestUri.AbsoluteUri == "https://localhost:7186/api/users/Send-Reset-Password-Email"
                        
                        ),ItExpr.IsAny<System.Threading.CancellationToken>()
                );
        }


        [Fact]
        public async Task SendResetCodeAsync_ShouldShowErrorMessage_WhenEmailIsEmpty()
        {
            // Arrange: No need to mock the HttpClient here as the email is empty
            _viewModel.Email = string.Empty;

            // Act: Call the SendResetCodeAsync method
            await _viewModel.SendResetCodeAsync();

            // Assert: Check if the ShowMessage event was triggered with the error message
            Assert.Equal("Please enter your email address.", _viewModel.Message);
        }

        [Fact]
        public async Task VerifyCodeANDsaveNewPasswordAsync_ShouldShowMessage_WhenPasswordIsValid()
        {
            // Arrange: Mock the SendAsync method to return a successful response
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK);

            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>
            (
                "SendAsync", ItExpr.Is<HttpRequestMessage>
                    (req => req.Method == HttpMethod.Post &&
                        req.RequestUri.AbsoluteUri == "https://localhost:7186/api/users/verify-code-and-change-password"
                    ), 
                    ItExpr.IsAny<System.Threading.CancellationToken>()
            )
            .ReturnsAsync(mockResponse);






            // Set test values
            _viewModel.Code = "123456";
            _viewModel.NewPassword = "newpassword";
            _viewModel.RepeatPassword = "newpassword";

            // Act: Call the VerifyCodeANDsaveNewPasswordAsync method
            await _viewModel.VerifyCodeANDsaveNewPasswordAsync();

            // Assert: Check if the ShowMessage event was triggered
            Assert.Contains("Password reset successfully.", _viewModel.Message);

            // Verify that SendAsync was called once with the correct parameters
            _mockHttpMessageHandler.Protected().Verify
                (
                    "SendAsync",
                    Times.Once(),
                    ItExpr.Is<HttpRequestMessage>
                        (
                            req => req.Method == HttpMethod.Post &&
                            req.RequestUri.AbsoluteUri == "https://localhost:7186/api/users/verify-code-and-change-password"

                        ), ItExpr.IsAny<System.Threading.CancellationToken>()
                );
        }

        [Fact]
        public async Task VerifyCodeANDsaveNewPasswordAsync_ShouldShowError_WhenPasswordsDoNotMatch()
        {
            // Arrange: Set up the view model with incorrect password and repeat password
            _viewModel.Code = "123456";
            _viewModel.NewPassword = "newpassword";
            _viewModel.RepeatPassword = "differentpassword";

            // Act: Call the VerifyCodeANDsaveNewPasswordAsync method
            await _viewModel.VerifyCodeANDsaveNewPasswordAsync();

            // Assert: Check if the ShowMessage event was triggered with the error message
            Assert.Contains("New passwords do not match.", _viewModel.Message);
        }

        [Fact]
        public async Task VerifyCodeANDsaveNewPasswordAsync_ShouldShowError_WhenCodeOrPasswordIsEmpty()
        {
            // Arrange: Set up the view model with missing code
            _viewModel.Code = string.Empty;
            _viewModel.NewPassword = "newpassword";
            _viewModel.RepeatPassword = "newpassword";

            // Act: Call the VerifyCodeANDsaveNewPasswordAsync method
            await _viewModel.VerifyCodeANDsaveNewPasswordAsync();

            // Assert: Check if the ShowMessage event was triggered with the error message
            Assert.Equal("Please fill in all fields", _viewModel.Message);
        }
    }
}

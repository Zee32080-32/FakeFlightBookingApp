using FakeFlightBookingApp.Model;
using FakeFlightBookingApp.ViewModel;
using Microsoft.Web.WebView2.Wpf;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FakeFlightBookingApp_Tests.ViewModelTests
{
    public class PaymentPageViewModelTests
    {
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _httpClient;
        private PaymentPageViewModel _viewModel;
        private Mock<Action<string>> _showMessageMock;
        private Mock<Action> _navigateToMainPageMock;
        private Mock<EventHandler<string>> _navigateToCheckoutUrlMock;

        public PaymentPageViewModelTests()
        {
            var flightOffer = new FlightOfferDTO
            {
                FlightNumber = "FL123",
                Price = "150.00",
                DepartureTime = "30/12/2024 10:00",
                ArrivalTime = "30/12/2024 12:00",
                Duration = "2h",
                AirlineName = "AirExample",
                Origin = "New York",
                Destination = "London",
                ClassType = "Economy",
                NumberOfTickets = 3
            };

            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:7186/")
            };

            _viewModel = new PaymentPageViewModel(flightOffer, _httpClient);

            //_showMessageMock = new Mock<Action<string>>();
            //_viewModel.StatusMessage += _showMessageMock.Object;

            _navigateToMainPageMock = new Mock<Action>();
            _viewModel.NavigateToMainPage += _navigateToMainPageMock.Object;

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

            _navigateToCheckoutUrlMock = new Mock<EventHandler<string>>();

        }

        [Fact]
        public async Task ProceedToPayment_ShouldReturnSuccess_WhenApiReturnsStatusCode200()
        {
            // Arrange: Set up a mock success response with status code 200
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"session\": \"https://checkout.example.com\"}", Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && req.RequestUri.AbsoluteUri == "https://localhost:7186/api/payment/create-checkout-session"),
                    ItExpr.IsAny<System.Threading.CancellationToken>()
                )
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _viewModel.ProceedToPayment();

            // Assert
            Assert.Equal("https://checkout.example.com", result);  // Update the expected URL here

        }

        [Fact]
        public async Task HandlePaymentSuccess_ShouldCallNavigateToMainPage_WhenBookingIsSuccessful()
        {
            // Arrange
            Application.Current.Properties["UserId"] = 1;

            var mockResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent("{\"session\": \"https://checkout.example.com\"}", Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri.AbsoluteUri == "https://localhost:7186/api/BookedFlight/AddBookedFlight"),
                    ItExpr.IsAny<System.Threading.CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            // Act
            await _viewModel.HandlePaymentSuccess();

            // Assert: Ensure the success message is shown
            Assert.Equal("Booking successful! Taking you to homepage", _viewModel.StatusMessage);
            //_showMessageMock.Verify(m => m(It.Is<string>(s => s.Contains("Booking successful!"))), Times.Once);

            // Assert: Ensure NavigateToMainPage is called after booking success
            _navigateToMainPageMock.Verify(m => m(), Times.Once);
        }

    }
}




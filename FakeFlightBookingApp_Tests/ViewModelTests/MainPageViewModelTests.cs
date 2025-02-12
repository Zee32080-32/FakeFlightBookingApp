using FakeFlightBookingApp;
using FakeFlightBookingApp.Model;
using FakeFlightBookingApp.ViewModel;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace FakeFlightBookingApp_Tests.ViewModelTests
{
    public class MainPageViewModelTests
    {
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _httpClient;
        private MainPageViewModel _viewModel;

        public MainPageViewModelTests()
        {
            // Mock HttpMessageHandler to intercept HttpClient calls
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new System.Uri("https://localhost:7186/api/users/")
            };

            // Initialize the ViewModel with the mocked HttpClient
            _viewModel = new MainPageViewModel(_httpClient);
        }

        [Fact]
        public async Task FetchFlightOffers_ShouldPopulateFlightOffers_WhenApiReturnsValidResponse()
        {
            // Arrange
            var mockFlightOfferResponse = new FlightOffersResponse
            {
                Data = new List<FlightOffer>
        {
            new FlightOffer
            {
                Itineraries = new List<Itinerary>
                {
                    new Itinerary
                    {
                        Segments = new List<Segment>
                        {
                            new Segment
                            {
                                Departure = new Departure
                                {
                                    IataCode = "JFK",
                                    At = DateTime.Now.AddHours(2)
                                },
                                Arrival = new Arrival
                                {
                                    IataCode = "LAX",
                                    At = DateTime.Now.AddHours(5)
                                },
                                Number = "AA123"
                            }
                        }
                    }
                },
                Price = new Price
                {
                    Total = "300.00"
                }
            }
        }
            };

            // Convert mock response to JSON and set it as the response content
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(mockFlightOfferResponse), Encoding.UTF8, "application/json")
            };

            // Mock the HttpClient to return the mockResponse for the specific URL
            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>
                (
                    req => req.Method == HttpMethod.Get && req.RequestUri.AbsoluteUri.Contains("/api/Flights/search")
                ),
                ItExpr.IsAny<System.Threading.CancellationToken>()
            ).ReturnsAsync(mockResponse);

            // Act: Set required properties for the search and call the method
            _viewModel.FlightFrom = "JFK";
            _viewModel.FlightTo = "LAX";
            _viewModel.DepartureDate = DateTime.Now.AddDays(1);
            _viewModel.NumberOfTickets = "1";
            _viewModel.SelectedTravelClass = "Economy";

            await _viewModel.ExecuteSearchFlightsCommand();

            // Assert: Ensure the FlightOffersCollection is populated
            Assert.Single(_viewModel.FlightOffersCollection); // Check if one flight offer is added
            var flightOffer = _viewModel.FlightOffersCollection[0]; // Get the first offer
            Assert.Equal("300.00", flightOffer.Price); // Check if the price matches
            Assert.Equal("AA123", flightOffer.FlightNumber); // Check if the flight number matches
            Assert.Equal("JFK", flightOffer.Origin); // Check if the origin matches
            Assert.Equal("LAX", flightOffer.Destination); // Check if the destination matches
        }
    }


}

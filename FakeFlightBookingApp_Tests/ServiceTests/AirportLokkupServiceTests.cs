using FakeFlightBookingAPI.Models;
using FakeFlightBookingAPI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeFlightBookingApp_Tests.ServiceTests
{
    public class AirportLokkupServiceTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly Mock<IOptions<AmadeusOptions>> _mockAmadeusOptions;
        private readonly Mock<ILogger<AirportLookupService>> _mockLogger;
        private readonly AirportLookupService _service;

        public AirportLokkupServiceTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _mockAmadeusOptions = new Mock<IOptions<AmadeusOptions>>();
            _mockLogger = new Mock<ILogger<AirportLookupService>>();

            var amadeusOptions = new AmadeusOptions { ApiKey = "test_api_key", ApiSecret = "test_api_secret" };
            _mockAmadeusOptions.Setup(o => o.Value).Returns(amadeusOptions);

            // Create HttpClient with mocked HttpMessageHandler
            var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://test.api.amadeus.com") // Set base address
            };

            _service = new AirportLookupService(httpClient, _mockAmadeusOptions.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task SearchAirportsAsync_ReturnsAirports_WhenApiResponseIsSuccessful()
        {
            // Arrange
            var query = "New York City";
            var expectedAirports = new List<Airport>
            {
                new Airport
                {
                    IataCode = "JFK",
                    Name = "John F. Kennedy International Airport",
                    City = "New York City",
                    Country = "United States (USA)"
                },
                new Airport
                {
                    IataCode = "LGA",
                    Name = "LaGuardia Airport",
                    City = "New York City",
                    Country = "United States (USA)"
                }
            };

            // Mock successful token response
            var tokenJson = "{\"access_token\": \"mock_token\"}";
            var mockTokenResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(tokenJson)
            };

            // Mock the token request to return a successful token
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.AbsoluteUri.Contains("/v1/security/oauth2/token")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockTokenResponse);

            // Mock successful API response for the airport search
            var jsonResponse = JsonConvert.SerializeObject(new AirportResponse { data = expectedAirports });
            var mockResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse)
            };

            // Setup the HttpMessageHandler to return the mock response for the airport search
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.AbsoluteUri.Contains("locations?subType=AIRPORT&keyword=")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _service.SearchAirportsAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedAirports.Count, result.Count);
            Assert.Equal(expectedAirports[0].IataCode, result[0].IataCode);
            Assert.Equal(expectedAirports[0].Name, result[0].Name);
        }

        [Fact]
        public async Task GetAccessTokenAsync_ReturnsToken_WhenApiResponseIsSuccessful()
        {
            // Arrange
            var expectedToken = "access_token_123";
            var tokenResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new Dictionary<string, string> { { "access_token", expectedToken } }))
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.AbsoluteUri.Contains("/v1/security/oauth2/token")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(tokenResponse);

            // Act
            var result = await _service.GetAccessTokenAsync();

            // Assert
            Assert.Equal(expectedToken, result);
        }



    }
}

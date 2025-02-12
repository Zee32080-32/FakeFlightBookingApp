using FakeFlightBookingAPI.Models;
using FakeFlightBookingAPI.Services;
using Microsoft.Extensions.Options;
using Moq.Protected;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FakeFlightBookingApp_Tests.ServiceTests
{
    public class FlightOffersSearchServiceTests
    {
        [Fact]
        public async Task GetAccessTokenAsync_Success_ReturnsAccessToken()
        {
            // Arrange
            var amadeusOptions = Options.Create(new AmadeusOptions
            {
                ApiKey = "fake_api_key",
                ApiSecret = "fake_api_secret",
                BaseUrl = "https://test.api.amadeus.com"
            });

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri.ToString().Contains("/v1/security/oauth2/token")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(new { access_token = "fake_access_token" }))
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri(amadeusOptions.Value.BaseUrl)
            };

            var flightService = new FlightOffersSearchService(httpClient, amadeusOptions);

            // Act
            var token = await flightService.GetAccessTokenAsync();

            // Assert
            Assert.Equal("fake_access_token", token);
        }

        [Fact]
        public async Task SearchFlightsAsync_Success_ReturnsHttpResponseMessage()
        {
            // Arrange
            var amadeusOptions = Options.Create(new AmadeusOptions
            {
                ApiKey = "fake_api_key",
                ApiSecret = "fake_api_secret",
                BaseUrl = "https://test.api.amadeus.com"
            });

            var criteria = new FlightSearchCriteria
            {
                From = "LAX",
                To = "JFK",
                DepartureDate = "2025-01-10",
                ReturnDate = "2025-01-20",
                NumberOfTickets = 1
            };

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            // Mock access token request
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri.ToString().Contains("/v1/security/oauth2/token")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(new { access_token = "fake_access_token" }))
                });

            // Mock flight offers request
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString().Contains("v2/shopping/flight-offers")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("Fake Flight Response")
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri(amadeusOptions.Value.BaseUrl)
            };

            var flightService = new FlightOffersSearchService(httpClient, amadeusOptions);

            // Act
            var response = await flightService.SearchFlightsAsync(criteria);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.Equal("Fake Flight Response", responseBody);

            // Verify the token request was made
            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri.ToString().Contains("/v1/security/oauth2/token")),
                ItExpr.IsAny<CancellationToken>());

            // Verify the flight search request was made
            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri.ToString().Contains("v2/shopping/flight-offers")),
                ItExpr.IsAny<CancellationToken>());
        }
    }
}

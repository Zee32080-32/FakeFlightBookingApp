using FakeFlightBookingAPI.Controllers;
using FakeFlightBookingAPI.Data;
using FakeFlightBookingAPI.Models;
using FakeFlightBookingAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeFlightBookingApp_Tests.ControllerTests
{
    public class FlightControllerTests
    {
        private readonly FlightsController _controller;
        private readonly IMemoryCache _memoryCache;
        private readonly Mock<IFlightOffersSearchService> _mockFlightOffersSearchService;
        private readonly Mock<IAirportLookupService> _mockAirportLookupService;
        private readonly FlightSearchCriteria _testFlightSearchCriteria;
        public FlightControllerTests()
        {
            
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _mockFlightOffersSearchService = new Mock<IFlightOffersSearchService>();
            _mockAirportLookupService = new Mock<IAirportLookupService>();

            _controller = new FlightsController(_mockFlightOffersSearchService.Object, _mockAirportLookupService.Object);

            _testFlightSearchCriteria = new FlightSearchCriteria
            {
                From = "TestFrom",
                To = "TestTo",
                DepartureDate = "2024-12-12",
                ReturnDate = "2024-12-18",
                NumberOfTickets = 2,
                TravelClass = "Business"
            };
        }

        [Fact]
        public async Task FlightController_SearchFlights_ReadJsonFileReturnOK()
        {
            // Arrange
            var testFlightSearchCriteria = _testFlightSearchCriteria;

            var mockResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            mockResponse.Content = new StringContent("[{ \"flightId\": 1, \"price\": 200 }]");

            // Mock the SearchFlightsAsync method to return a successful response
            _mockFlightOffersSearchService
                .Setup(service => service.SearchFlightsAsync(It.IsAny<FlightSearchCriteria>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.SearchFlights(testFlightSearchCriteria);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task FlightController_SearchAirports_ReturnListOfAirports()
        {
            //Arrange
            string keyword = "NYC";
            var airports = new List<Airport>
            {
                new Airport 
                {   IataCode = "JFK", 
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
            _mockAirportLookupService.Setup(service => service.SearchAirportsAsync(It.IsAny<string>())).ReturnsAsync(airports);

            //Act
            var result = await _controller.SearchAirports(keyword);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(airports, okResult.Value);
        }


    }


}

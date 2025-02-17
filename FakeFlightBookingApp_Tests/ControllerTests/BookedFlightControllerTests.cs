using FakeFlightBookingAPI.Controllers;
using FakeFlightBookingAPI.Data;
using FakeFlightBookingAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using SharedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace FakeFlightBookingApp_Tests.ControllerTests
{
    public class BookedFlightControllerTests : IDisposable
    {
        private readonly BookedFlightController _controller;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMemoryCache _memoryCache;
        private readonly BookedFlight _testBookedFlight;
        private readonly Customer _testCustomer;

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted(); // Deletes the in-memory database
            _dbContext.Dispose(); // Disposes the DbContext
        }
        public BookedFlightControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                       .UseInMemoryDatabase("TestBookedFlightDatabase")
                       .Options;
            _dbContext = new ApplicationDbContext(options);
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
           
            _controller = new BookedFlightController(_dbContext, _memoryCache);

            _testBookedFlight = new BookedFlight
            {
                CustomerId = 1,
                FlightNumber = "222",
                AirlineName = "AirlineName",
                Origin = "TestOrigin",
                Destination = "TestDestination",
                DepartureDateTime = DateTime.ParseExact("12/12/2024 08:33", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                ArrivalDateTime = DateTime.ParseExact("18/12/2024 03:12", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                Price = decimal.Parse("805.67"),
                NumberOfTickets = 2,
                ClassType = "Business"?.Trim(),
                BookingDate = DateTime.UtcNow // ISO 8601 format
            };

            _testCustomer = new Customer("John", "Doe", "JohnDoe", "johnDoe@TestEmail.com", "password123", "0123456789");

        }




        [Fact]
        public async Task BookedFlightController_GetBookedFlightByID_ReturnsListOfBookedFlights()
        {
            // Arrange
            _dbContext.CustomerUsers.Add(_testCustomer);
            await _dbContext.SaveChangesAsync();

            _testBookedFlight.CustomerId = _testCustomer.Id; // Ensure it matches the test customer ID
            _dbContext.BookedFlights.Add(_testBookedFlight);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _controller.GetBookedFlightByID(_testCustomer.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var bookedFlights = Assert.IsType<List<BookedFlight>>(okResult.Value);

            Assert.Single(bookedFlights);
            Assert.Equal(_testBookedFlight.FlightNumber, bookedFlights[0].FlightNumber);
        }


        [Fact]
        public async Task BookedFlightController_PostBookedFlight_AddBookedFlight()
        {
            // Arrange

            (string hashedPassword, string salt) = PasswordHelper.HashPassword(_testCustomer.Password);
            int ID = 1;

            _testCustomer.Id = ID;
            _testCustomer.Password = hashedPassword;
            _testCustomer.Salt = salt;


            var testFlight = _testBookedFlight;
            var testCustomer = _testCustomer;

            _dbContext.CustomerUsers.Add(_testCustomer);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _controller.PostBookedFlight(testFlight);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(201, createdResult.StatusCode);

            var addedFlight = await _dbContext.BookedFlights.FindAsync(_testBookedFlight.BookingId);
            Assert.NotNull(addedFlight);
            Assert.Equal(_testBookedFlight.CustomerId, addedFlight.CustomerId);
        }
       
        [Fact]
        public async Task BookedFlightController_PutBookedFlight_UpdateBookedFlight()
        {
            // Arrange
            (string hashedPassword, string salt) = PasswordHelper.HashPassword(_testCustomer.Password);
            int ID = 1;

            _testCustomer.Id = ID;
            _testCustomer.Password = hashedPassword;
            _testCustomer.Salt = salt;

            var testCustomer = _testCustomer;
            var testBookedFlight = _testBookedFlight;

            _dbContext.CustomerUsers.Add(_testCustomer);
            await _dbContext.SaveChangesAsync();

            _dbContext.BookedFlights.Add(testBookedFlight);
            await _dbContext.SaveChangesAsync();

            // Fetch the existing flight from the DB (avoids conflict with the new one)
            var existingFlight = await _dbContext.BookedFlights.FindAsync(testBookedFlight.BookingId);

            // Modify the flight details
            existingFlight.FlightNumber = "444";
            existingFlight.NumberOfTickets = 3;
            existingFlight.Price = decimal.Parse("1000.67");
            existingFlight.ClassType = "Business";
            existingFlight.DepartureDateTime = DateTime.ParseExact("20/12/2024 08:33", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            existingFlight.ArrivalDateTime = DateTime.ParseExact("29/01/2024 03:12", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

            // Act
            var result = await _controller.PutBookedFlight(existingFlight.BookingId, existingFlight);

            // Assert
            Assert.IsType<NoContentResult>(result);

            var updatedFlight = await _dbContext.BookedFlights.FindAsync(existingFlight.BookingId);
            Assert.NotNull(updatedFlight);
            Assert.Equal("444", updatedFlight.FlightNumber);
            Assert.Equal(3, updatedFlight.NumberOfTickets);
            Assert.Equal(decimal.Parse("1000.67"), updatedFlight.Price);
        }

        [Fact]
        public async Task BookedFlightController_DeleteBookedFlight_RemoveBookedFlightByID()
        {
            // Arrange
            (string hashedPassword, string salt) = PasswordHelper.HashPassword(_testCustomer.Password);
            int ID = 1;

            _testCustomer.Id = ID;
            _testCustomer.Password = hashedPassword;
            _testCustomer.Salt = salt;

            var testCustomer = _testCustomer;
            var testBookedFlight = _testBookedFlight;

            _dbContext.CustomerUsers.Add(_testCustomer);
            await _dbContext.SaveChangesAsync();

            _dbContext.BookedFlights.Add(testBookedFlight);
            await _dbContext.SaveChangesAsync();

            //Act 
            var result = await _controller.DeleteBookedFlight(testBookedFlight.BookingId);

            //Assert
            Assert.IsType<NoContentResult>(result);

            // Verify that the flight no longer exists in the database
            var deletedFlight = await _dbContext.BookedFlights.FindAsync(testBookedFlight.BookingId);
            Assert.Null(deletedFlight);  // Ensure the flight is actually deleted
        }

    }
}

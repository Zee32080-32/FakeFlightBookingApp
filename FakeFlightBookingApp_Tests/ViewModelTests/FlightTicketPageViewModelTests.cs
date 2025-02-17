using FakeFlightBookingApp.ViewModel;
using Moq;
using Xunit;
using SharedModels;
using System.Collections.Generic;
using System.Windows;
using FakeFlightBookingApp.Model;
using FakeFlightBookingApp.View;
using System.Windows.Navigation;

namespace FakeFlightBookingApp.Tests
{
    public class FlightTicketPageViewModelTests
    {
        /*
        private readonly Mock<Action<string>> _showMessageMock;
        private readonly FlightTicketPageViewModel _viewModel;

        public FlightTicketPageViewModelTests()
        {
            _showMessageMock = new Mock<Action<string>>();

            // Provide realistic data for the FlightOfferDTO
            var flightOffers = new List<FlightOfferDTO>
        {
            new FlightOfferDTO
            {
                FlightNumber = "FL123",
                Price = "150.00",
                DepartureTime = "2024-12-30 10:00",
                ArrivalTime = "2024-12-30 12:00",
                Duration = "2h",
                AirlineName = "AirExample",
                Origin = "New York",
                Destination = "London",
                ClassType = "Economy",
                NumberOfTickets = 100
            }
        };

            _viewModel = new FlightTicketPageViewModel(flightOffers, "New York to London");
            _viewModel.ShowMessage += _showMessageMock.Object;
        }

        [Fact]
        public void ExecuteBuyTicketCommand_ValidFlight_ShouldInvokeShowMessage()
        {
            // Arrange
            var flightToBuy = new FlightOfferDTO
            {
                FlightNumber = "FL123",
                Price = "150.00",
                DepartureTime = "2024-12-30 10:00",
                ArrivalTime = "2024-12-30 12:00",
                Duration = "2h",
                AirlineName = "AirExample",
                Origin = "New York",
                Destination = "London",
                ClassType = "Economy",
                NumberOfTickets = 100
            };

            // Act
            _viewModel.BuyTicketCommand.Execute(flightToBuy);

            // Assert: Ensure ShowMessage was called with the correct message
            _showMessageMock.Verify(s => s.Invoke($"Buying ticket for flight {flightToBuy.FlightNumber}"), Times.Once);
        }

        [Fact]
        public void ExecuteBuyTicketCommand_NoFlightSelected_ShouldInvokeShowMessage()
        {
            // Arrange: Pass null as the flight
            FlightOfferDTO nullFlight = null;

            // Act
            _viewModel.BuyTicketCommand.Execute(nullFlight);

            // Assert: Ensure ShowMessage was called with the message for no selection
            _showMessageMock.Verify(s => s.Invoke("Please select a flight to buy a ticket."), Times.Once);
        }
        */
    }
}

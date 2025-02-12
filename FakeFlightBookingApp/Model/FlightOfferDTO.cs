using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeFlightBookingApp.Model
{
    public class FlightOfferDTO
    {
        public string FlightNumber { get; set; }
        public string Price { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
        public string Duration { get; set; }
        public string AirlineName { get; set; }  // Airline operating the flight
        public string Origin { get; set; }      // Departure location
        public string Destination { get; set; } // Arrival location
        public string ClassType { get; set; }

        public int NumberOfTickets { get; set; }
    }
}

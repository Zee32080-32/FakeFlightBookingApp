using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedModels
{
    public class BookedFlight
    {
        [Key]
        public int BookingId { get; set; } // Primary Key
       
        [ForeignKey("Customer")]
        public int CustomerId { get; set; } // Foreign Key to Customer Account 
        public string FlightNumber { get; set; }
        public string AirlineName { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureDateTime { get; set; }
        public DateTime ArrivalDateTime { get; set; }
        public decimal Price { get; set; }
        public int NumberOfTickets { get; set; }
        public string ClassType { get; set; }
        public DateTime BookingDate { get; set; }

    }
}

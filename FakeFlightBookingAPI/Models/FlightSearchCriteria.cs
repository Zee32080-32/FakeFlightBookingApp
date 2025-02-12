namespace FakeFlightBookingAPI.Models
{
    public class FlightSearchCriteria
    {
        public string From { get; set; }
        public string To { get; set; }
        public string DepartureDate { get; set; }
        public string? ReturnDate { get; set; }
        public int NumberOfTickets { get; set; }
        public string TravelClass { get; set; }

    }
}

namespace FakeFlightBookingAPI.Services
{
    //configuration for the Amadeus API based on my appsettings.json
    public class AmadeusOptions
    {
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }

        public string BaseUrl { get; set; } = "https://test.api.amadeus.com/v2"; // Change to production URL when ready


    }
}

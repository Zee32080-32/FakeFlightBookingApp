using FakeFlightBookingAPI.Services; // Importing the services used in the controller
using Microsoft.AspNetCore.Mvc; // For the base controller class
using System.Net.Http.Headers; // For AuthenticationHeaderValue
using System.Net.Http; // For HttpClient
using System.Threading.Tasks; // For async/await

namespace FakeFlightBookingAPI.Controllers
{
    // Define a route for the Flights API controller
    [Route("api/[controller]")]
    [ApiController] // Indicates that this class is an API controller
    public class FlightsController : ControllerBase
    {
        private readonly IFlightOffersSearchService _flightOffersSearchService; // Service for searching flight offers
        private readonly IAirportLookupService _airportLookupService; // Service for airport lookups

        // Constructor to inject the necessary services
        public FlightsController(IFlightOffersSearchService flightOffersSearchService, IAirportLookupService airportLookupService)
        {
            _flightOffersSearchService = flightOffersSearchService; // Initialize flight search service
            _airportLookupService = airportLookupService; // Initialize airport lookup service

        }

        // GET api/flights/search - Searches for flights based on criteria
        [HttpGet("search")]
        public async Task<IActionResult> SearchFlights([FromQuery] Models.FlightSearchCriteria criteria)
        {
            // Log incoming parameters for debugging
            Console.WriteLine($"Received Search Request: From={criteria.From}, To={criteria.To}, DepartureDate={criteria.DepartureDate}, ReturnDate={criteria.ReturnDate}, NumberOfTickets={criteria.NumberOfTickets}, TravelClass={criteria.TravelClass}");

            // Check if parameters are valid
            if (string.IsNullOrWhiteSpace(criteria.From) || string.IsNullOrWhiteSpace(criteria.To))
            {
                return BadRequest("From and To locations are required.");
            }

            if (!DateTime.TryParse(criteria.DepartureDate, out _))
            {
                return BadRequest("Invalid DepartureDate format.");
            }

            if (criteria.NumberOfTickets <= 0)
            {
                return BadRequest("Number of tickets must be greater than zero.");
            }

            // Call the service to search flights
            var response = await _flightOffersSearchService.SearchFlightsAsync(criteria);

            // Log response content even on failure
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error Response: {errorContent}");
                return StatusCode((int)response.StatusCode, "Error retrieving flights.");
            }

            var flightOffers = await response.Content.ReadFromJsonAsync<dynamic>();
            return Ok(flightOffers);
        }

        // GET api/flights/Search-Airports - Searches for airports based on a keyword
        [HttpGet("Search-Airports")]
        public async Task<IActionResult> SearchAirports([FromQuery] string keyword)
        {
            // Check if the keyword is provided
            if (string.IsNullOrWhiteSpace(keyword))
            {
                // Return a bad request response if keyword is missing
                return BadRequest("Keyword is required.");
            }

            // Call the airport lookup service to get matching airports
            var airports = await _airportLookupService.SearchAirportsAsync(keyword);
            return Ok(airports); // Return the list of airports with 200 OK status
        }
    }
}

using Microsoft.AspNetCore.WebUtilities; // For query string manipulation
using Microsoft.Extensions.Options; // For options pattern
using Newtonsoft.Json; // For JSON serialization and deserialization
using System.Net.Http.Headers; // For AuthenticationHeaderValue
using System.Net.Http; // For HttpClient
using System.Threading.Tasks; // For async/await

namespace FakeFlightBookingAPI.Services
{
    // This service interacts with the Amadeus FlightOffersSearch API
    public class FlightOffersSearchService : IFlightOffersSearchService
    {
        private readonly HttpClient _httpClient; // HttpClient for making requests
        private readonly AmadeusOptions _amadeusOptions; // Configuration options for Amadeus API

        // Constructor to initialize the service with HttpClient and Amadeus options
        public FlightOffersSearchService(HttpClient httpClient, IOptions<AmadeusOptions> amadeusOptions)
        {
            _httpClient = httpClient; // Assign the injected HttpClient
            _amadeusOptions = amadeusOptions.Value; // Get the options values
            _httpClient.BaseAddress = new Uri(_amadeusOptions.BaseUrl); // Set the base URL for Amadeus API
            _httpClient.Timeout = TimeSpan.FromSeconds(15); // Set a timeout for requests
        }

        // Expose HttpClient if needed elsewhere; otherwise, it can be omitted
        public HttpClient HttpClient => _httpClient;

        // Method to get an access token from the Amadeus API
        public async Task<string> GetAccessTokenAsync()
        {
            // Create a request to get the token without Authorization header
            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "/v1/security/oauth2/token");
            tokenRequest.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", _amadeusOptions.ApiKey),  // API key from options
                new KeyValuePair<string, string>("client_secret", _amadeusOptions.ApiSecret) // API secret from options
            });

            // Send the request to obtain the token
            var tokenResponse = await _httpClient.SendAsync(tokenRequest);

            // Check if the token request was successful
            if (tokenResponse.IsSuccessStatusCode)
            {
                var tokenContent = await tokenResponse.Content.ReadAsStringAsync(); // Read the response content
                var tokenJson = JsonConvert.DeserializeObject<Dictionary<string, string>>(tokenContent); // Deserialize to get the token
                return tokenJson["access_token"]; // Return the access token
            }

            // Throw an exception if token retrieval fails
            throw new Exception($"Failed to retrieve access token: {tokenResponse.ReasonPhrase}");
        }

        // Method to search for flights based on given criteria
        public async Task<HttpResponseMessage> SearchFlightsAsync(Models.FlightSearchCriteria criteria)
        {
            var token = await GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var queryParams = new Dictionary<string, string>
            {
                {"originLocationCode", criteria.From},
                {"destinationLocationCode", criteria.To},
                {"departureDate", criteria.DepartureDate},
                {"adults", criteria.NumberOfTickets.ToString()}
            };

            if (!string.IsNullOrEmpty(criteria.ReturnDate))
            {
                queryParams.Add("returnDate", criteria.ReturnDate);
            }

            // Log the full request URL
            var requestUrl = QueryHelpers.AddQueryString("v2/shopping/flight-offers", queryParams);
            Console.WriteLine($"Request URL: {requestUrl}");

            var response = await _httpClient.GetAsync(requestUrl);

            // Log response content even on failure
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response Status: {response.StatusCode}, Body: {responseBody}");

            return response;
        }

    }
}

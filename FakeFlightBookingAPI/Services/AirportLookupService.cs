using FakeFlightBookingAPI.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;

namespace FakeFlightBookingAPI.Services
{
    public class AirportLookupService : IAirportLookupService
    {
        private readonly HttpClient _httpClient;
        private readonly AmadeusOptions _amadeusOptions;
        private readonly ILogger<AirportLookupService> _logger;

        public AirportLookupService(HttpClient httpClient, IOptions<AmadeusOptions> amadeusOptions, ILogger<AirportLookupService> logger)
        {
            _httpClient = httpClient;
            _amadeusOptions = amadeusOptions.Value;
            _logger = logger;

            //potentially delete this if I get errors
            _httpClient.BaseAddress = new Uri("https://test.api.amadeus.com");

        }
        public async Task<List<Airport>> SearchAirportsAsync(string query)
        {
            // Step 1: Get the access token (to authenticate with Amadeus API)
            var token = await GetAccessTokenAsync();

            // Now set the Authorization header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            // Step 2: Make the API call to search airports
            var response = await _httpClient.GetAsync($"https://test.api.amadeus.com/v1/reference-data/locations?subType=AIRPORT&keyword={query}");

            // Step 3: Ensure the response is successful
            response.EnsureSuccessStatusCode();

            // Step 4: Deserialize the response into a list of Airport objects
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var airportData = JsonConvert.DeserializeObject<AirportResponse>(jsonResponse); // Make sure to use JsonConvert

            return airportData?.data;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            // Create the token request without any Authorization header
            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "/v1/security/oauth2/token");

            // Log the request details for debugging
            Console.WriteLine("Request URI: " + tokenRequest.RequestUri);
            Console.WriteLine("Method: " + tokenRequest.Method);

            // Set the content for the token request
            tokenRequest.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", _amadeusOptions.ApiKey),
                new KeyValuePair<string, string>("client_secret", _amadeusOptions.ApiSecret)
            });

            // Log the content being sent
            var content = await tokenRequest.Content.ReadAsStringAsync();
            Console.WriteLine("Content: " + content);

            // Send the request
            var tokenResponse = await _httpClient.SendAsync(tokenRequest);

            // Check the response
            if (tokenResponse.IsSuccessStatusCode)
            {
                var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
                var tokenJson = JsonConvert.DeserializeObject<Dictionary<string, string>>(tokenContent);
                return tokenJson["access_token"];
            }
            else
            {
                var errorContent = await tokenResponse.Content.ReadAsStringAsync();
                throw new Exception($"Failed to retrieve access token: {tokenResponse.ReasonPhrase}. Error: {errorContent}");
            }
        }

    }
}



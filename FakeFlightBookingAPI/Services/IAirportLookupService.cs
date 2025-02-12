using FakeFlightBookingAPI.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace FakeFlightBookingAPI.Services
{
    public interface IAirportLookupService
    {
        Task<List<Airport>> SearchAirportsAsync(string query);


        Task<string> GetAccessTokenAsync();
 
    }
}

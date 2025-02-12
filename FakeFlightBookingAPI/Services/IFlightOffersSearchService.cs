namespace FakeFlightBookingAPI.Services
{
    public interface IFlightOffersSearchService
    {
        Task<string> GetAccessTokenAsync();
        Task<HttpResponseMessage> SearchFlightsAsync(Models.FlightSearchCriteria criteria);
    }
}

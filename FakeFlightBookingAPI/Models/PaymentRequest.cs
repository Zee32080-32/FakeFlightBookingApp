namespace FakeFlightBookingAPI.Models
{
    public class PaymentRequest
    {
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public long Amount { get; set; }
        public string? Currency { get; set; } = "usd";
    }
}

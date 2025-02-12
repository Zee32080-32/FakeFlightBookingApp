namespace FakeFlightBookingAPI.Models
{
    public class EmailVerification
    {
        public required string Email { get; set; }
        public required string Code { get; set; }
    }
}

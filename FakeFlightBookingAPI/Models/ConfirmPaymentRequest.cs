namespace FakeFlightBookingAPI.Models
{
    public class ConfirmPaymentRequest
    {
        public string PaymentIntentId { get; set; }
        public string PaymentMethodId { get; set; }
    }
}

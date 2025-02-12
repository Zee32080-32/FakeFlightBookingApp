using Stripe;
using Stripe.Checkout;

namespace FakeFlightBookingAPI.Services
{
    public interface IPaymentService
    {
        Task<string> CreateCheckoutSessionAsync(string successUrl, string cancelUrl, string amount);

        Task HandleWebhookEventAsync(Event stripeEvent);
       
    }
}


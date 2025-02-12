using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Stripe;
using Stripe.BillingPortal;
using Stripe.Checkout;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Forms;

namespace FakeFlightBookingAPI.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly StripeOptions _stripeOptions;
        public Func<Stripe.Checkout.SessionService> SessionServiceFactory { get; set; } = () => new Stripe.Checkout.SessionService();

        public PaymentService(IOptions<StripeOptions> stripeOptions)
        {
            _stripeOptions = stripeOptions.Value;
            StripeConfiguration.ApiKey = _stripeOptions.SecretKey;
        }

        /// <summary>
        /// Create a one-time payment checkout session.
        /// </summary>
        /// <param name="successUrl">URL to redirect on successful payment</param>
        /// <param name="cancelUrl">URL to redirect on payment cancellation</param>
        /// <param name="amount">Payment amount in USD</param>
        /// <returns>Stripe Checkout Session</returns>
        public async Task<string> CreateCheckoutSessionAsync(string successUrl, string cancelUrl, string amount)
        {
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (int)(decimal.Parse(amount) * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Flight Ticket",
                                Description = "One-time purchase of flight ticket"
                            },
                        },
                        Quantity = 1,
                    }
                },
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
            };

            //var service = new Stripe.Checkout.SessionService();
            var service = SessionServiceFactory();
            var session = await service.CreateAsync(options);
            return session.Url; 
        }

        /// <summary>
        /// Handle webhook events like payment success or failure.
        /// </summary>
        /// <param name="stripeEvent">Stripe event payload</param>
        /// <returns>Task</returns>
        public async Task HandleWebhookEventAsync(Event stripeEvent)
        {
            switch (stripeEvent.Type)
            {
                case "checkout.session.completed":
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                    // Handle payment success logic
                    break;

                case "checkout.session.expired":
                    // Handle session expiration
                    break;

                case "payment_intent.payment_failed":
                    // Handle payment failure
                    break;

                default:
                    break;
            }

            await Task.CompletedTask;
        }


    }
}
   


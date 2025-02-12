using FakeFlightBookingAPI.Data;
using FakeFlightBookingAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FakeFlightBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] Models.CheckOutSeesionRequest request)
        {
            var session = await _paymentService.CreateCheckoutSessionAsync(
                "https://localhost:7186/api/payment/success?sessionId={CHECKOUT_SESSION_ID}",
                "https://localhost:7186/api/payment/cancel?sessionId={CHECKOUT_SESSION_ID}",
                request.Amount
            );

            return Ok(new { session });

        }


        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    "whsec_14676b6ea1cadcd04be990ce4fd10865021adf7a255abc41686c0ff497799cf5"
                );

                // Handle specific event types
                switch (stripeEvent.Type)
                {
                    case "checkout.session.completed":
                        var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                        // Handle checkout session completion
                        break;

                    case "payment_intent.succeeded":
                        var paymentIntent = stripeEvent.Data.Object as Stripe.PaymentIntent;
                        // Handle successful payment
                        break;

                    case "payment_intent.payment_failed":
                        // Handle failed payment
                        break;

                    default:
                        break;
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("success")]
        public async Task<IActionResult> Success(string sessionId)
        {
            var service = new Stripe.Checkout.SessionService();
            var session = await service.GetAsync(sessionId);

            if (session.PaymentStatus == "paid")
            {
                // Return a simple HTML page with the success message
                return Content(@"
                <html>
                    <head>
                        <title>Payment Success</title>
                    </head>
                    <body>
                        <h1>Payment completed successfully!</h1>
                        <p>Thank you for your purchase. Your order is being processed.</p>
                        <a href='/'>Go back to home page</a>
                    </body>
                </html>
            ", "text/html");
            }

            return Content("Payment is still processing. Please wait or contact support.");
        }

        [HttpGet("cancel")]
        public IActionResult Cancel(string sessionId)
        {
            return Content("Payment was canceled. You can try again or contact support.");
        }
    }


}

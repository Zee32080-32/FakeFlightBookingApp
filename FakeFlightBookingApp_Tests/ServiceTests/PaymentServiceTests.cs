using FakeFlightBookingAPI.Services;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stripe;
using Stripe.Checkout;


namespace FakeFlightBookingApp_Tests.ServiceTests
{
    public class PaymentServiceTests
    {
        [Fact]
        public async Task CreateCheckoutSessionAsync_ValidInput_ReturnsSessionUrl()
        {
            // Arrange
            var stripeOptions = Options.Create(new StripeOptions
            {
                SecretKey = "fake_secret_key"
            });

            var mockSessionService = new Mock<Stripe.Checkout.SessionService>();
            var fakeSessionUrl = "https://checkout.stripe.com/pay/fake_session_url";

            mockSessionService
                .Setup(service => service.CreateAsync(It.IsAny<SessionCreateOptions>(), null, default))
                .ReturnsAsync(new Session { Url = fakeSessionUrl });

            var paymentService = new PaymentService(stripeOptions)
            {
                SessionServiceFactory = () => mockSessionService.Object // Dependency injection for testing
            };

            string successUrl = "https://example.com/success";
            string cancelUrl = "https://example.com/cancel";
            string amount = "100.50";

            // Act
            var sessionUrl = await paymentService.CreateCheckoutSessionAsync(successUrl, cancelUrl, amount);

            // Assert
            Assert.Equal(fakeSessionUrl, sessionUrl);
            mockSessionService.Verify(service =>
                service.CreateAsync(It.IsAny<SessionCreateOptions>(), null, default), Times.Once);
        }

        [Fact]
        public async Task HandleWebhookEventAsync_CheckoutSessionCompleted_HandlesEventCorrectly()
        {
            // Arrange
            var stripeOptions = Options.Create(new StripeOptions
            {
                SecretKey = "fake_secret_key"
            });

            var paymentService = new PaymentService(stripeOptions);

            var stripeEvent = new Event
            {
                Type = "checkout.session.completed",
                Data = new EventData
                {
                    Object = new Session
                    {
                        Id = "cs_test_123",
                        CustomerEmail = "test@example.com",
                    }
                }
            };

            // Act
            await paymentService.HandleWebhookEventAsync(stripeEvent);

            // Assert
            // No exception thrown indicates successful handling
            Assert.True(true);
        }

    }
}

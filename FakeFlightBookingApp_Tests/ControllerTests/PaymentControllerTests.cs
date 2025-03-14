﻿using FakeFlightBookingAPI.Controllers;
using FakeFlightBookingAPI.Models;
using FakeFlightBookingAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeFlightBookingAPI.Models;
using Microsoft.AspNetCore.Http;
using Stripe;
using Stripe.Checkout;
using FluentAssertions;

namespace FakeFlightBookingApp_Tests.ControllerTests
{
    public class PaymentControllerTests
    {
        private readonly PaymentController _controller;
        private readonly Mock<IPaymentService> _mockPaymentService;

        public PaymentControllerTests()
        {
            _mockPaymentService = new Mock<IPaymentService>();

            _controller = new PaymentController(_mockPaymentService.Object);

        }

        [Fact]
        public async Task CreateCheckoutSession_ReturnsOkResult_WithSession()
        {


        }

        [Fact]
        public async Task Webhook_HandlesCheckoutSessionCompletedEvent_ReturnsOk()
        {
   
        }

        [Fact]
        public async Task Success_ReturnsSuccessPage_WhenPaymentStatusIsPaid()
        {
          
        }
    }
}

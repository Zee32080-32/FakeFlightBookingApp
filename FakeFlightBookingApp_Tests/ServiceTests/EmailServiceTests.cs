using FakeFlightBookingAPI.Services;
using Microsoft.Extensions.Options;
using Moq;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FakeFlightBookingApp_Tests.ServiceTests
{
    public  class EmailServiceTests
    {
        [Fact]
        public async Task SendEmailAsync_SuccessfulResponse_NoExceptionThrown()
        {
            // Arrange
            var sendGridOptions = Options.Create(new SendGridOptions
            {
                ApiKey = "fake_api_key",
                FromEmail = "test@example.com"
            });

            var mockSendGridClient = new Mock<ISendGridClient>();
            var mockResponse = new Response(HttpStatusCode.Accepted, null, null);

            mockSendGridClient
                .Setup(client => client.SendEmailAsync(It.IsAny<SendGridMessage>(), default))
                .ReturnsAsync(mockResponse);

            var emailService = new EmailService(sendGridOptions, mockSendGridClient.Object);

            // Act
            var exception = await Record.ExceptionAsync(() =>
                emailService.SendEmailAsync("recipient@example.com", "Test Subject", "Test Body"));

            // Assert
            Assert.Null(exception); // No exception should be thrown for a successful response
            mockSendGridClient.Verify(client => client.SendEmailAsync(It.IsAny<SendGridMessage>(), default), Times.Once);
        }

        [Fact]
        public async Task SendEmailAsync_UnsuccessfulResponse_ThrowsException()
        {
            // Arrange
            var sendGridOptions = Options.Create(new SendGridOptions
            {
                ApiKey = "fake_api_key",
                FromEmail = "test@example.com"
            });

            var mockSendGridClient = new Mock<ISendGridClient>();
            var mockResponse = new Response(HttpStatusCode.BadRequest, null, null);

            mockSendGridClient
                .Setup(client => client.SendEmailAsync(It.IsAny<SendGridMessage>(), default))
                .ReturnsAsync(mockResponse);

            var emailService = new EmailService(sendGridOptions, mockSendGridClient.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                emailService.SendEmailAsync("recipient@example.com", "Test Subject", "Test Body"));

            Assert.Equal("Email sending failed with status code: BadRequest", exception.Message);
            mockSendGridClient.Verify(client => client.SendEmailAsync(It.IsAny<SendGridMessage>(), default), Times.Once);
        }
    }
}

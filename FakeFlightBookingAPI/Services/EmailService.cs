using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Threading.Tasks;
using static System.Windows.Forms.Design.AxImporter;


//interacts with SendGrid API
namespace FakeFlightBookingAPI.Services
{
    public class EmailService: IEmailService
    {
        private readonly SendGridOptions _sendGridOptions;
        private readonly ISendGridClient _sendGridClient;

        public EmailService(IOptions<SendGridOptions> sendGridOptions, ISendGridClient sendGridClient = null)
        {
            _sendGridOptions = sendGridOptions.Value;

            _sendGridClient = sendGridClient ?? new SendGridClient(_sendGridOptions.ApiKey);

        }

        // asynchronous, allowing it to perform non-blocking operations
        public async Task SendEmailAsync(string toEmail, string subject, string messageBody)
        {
            var from = new EmailAddress(_sendGridOptions.FromEmail);
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, messageBody, messageBody);

            var response = await _sendGridClient.SendEmailAsync(msg);


            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Accepted)
            {
                throw new Exception($"Email sending failed with status code: {response.StatusCode}");
            }

        }
    }
}

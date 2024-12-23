using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Manage.Shared
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ISendGridClient _sendGridClient;

        public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger, ISendGridClient sendGridClient)
        {
            _configuration = configuration;
            _logger = logger;
            _sendGridClient = sendGridClient;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_configuration["SendGrid:From"], _configuration["SendGrid:Name"]),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(toEmail));

            var response = await _sendGridClient.SendEmailAsync(msg);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"Email to {toEmail} queued successfully!");
            }
            else
            {
                var errorContent = await response.Body.ReadAsStringAsync();
                _logger.LogError($"Failed to send email to {toEmail}: {response.StatusCode}, {errorContent}");
            }
        }

    }
}

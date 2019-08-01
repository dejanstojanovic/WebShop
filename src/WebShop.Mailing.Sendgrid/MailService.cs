using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Mailing.Sendgrid
{
    public class MailService:IMailService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly ISendGridClient _sendgridClient;

        private MailServiceConfiguration _mailServiceConfiguration;
        public MailService(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _mailServiceConfiguration = new MailServiceConfiguration();
            _configuration.Bind("SendGrid",_mailServiceConfiguration);


            _sendgridClient = new SendGridClient(_mailServiceConfiguration.ApiKey);
        }

        public async Task SendAsync(MailMessage message)
        {
            var msg = new SendGrid.Helpers.Mail.SendGridMessage()
            {
                HtmlContent = message.Body,
                From = String.IsNullOrWhiteSpace(message.From) new EmailAddress(message.From),
                Subject = message.Subject,
            };
            msg.AddTos(message.To.Select(t => new EmailAddress(t)).ToList());
            await _sendgridClient.SendEmailAsync(msg);
        }

        public void Dispose()
        {

        }
    }
}

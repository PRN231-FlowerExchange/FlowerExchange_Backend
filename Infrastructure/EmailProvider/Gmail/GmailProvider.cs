using Domain.EmailProvider;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.EmailProvider.Gmail
{
    public class GmailProvider : IEmailProvider
    {
        private readonly GmailOptions _options;
        public GmailProvider(IOptions<GmailOptions> options)
        {
            if (options.Value == null)
            {
                throw new ArgumentNullException(nameof(options.Value));
            }
            _options = options.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = _options.SmtpServer;
            smtp.Port = _options.Port;
            smtp.Credentials = new NetworkCredential(
                userName: _options.AccountName,
                password: _options.Password);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;

            //Set up message
            MailAddress to = new MailAddress(email);
            MailAddress from = new MailAddress(
                displayName: _options.FromDisplayName,
                address: _options.FromEmailAddress);
            MailMessage content = new MailMessage(from, to);
            content.Subject = subject;
            content.Body = htmlMessage;
            content.IsBodyHtml = true;

            await smtp.SendMailAsync(content);
            smtp.Dispose();
        }
    }
}

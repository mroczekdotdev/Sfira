using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Services.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSenderOptions options;

        public EmailSender(IOptionsMonitor<EmailSenderOptions> optionsAccessor)
        {
            options = optionsAccessor.CurrentValue;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var credentials = new NetworkCredential(options.Username, options.Password);

            var mail = new MailMessage()
            {
                From = new MailAddress(options.Username, options.SenderName),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            mail.To.Add(new MailAddress(email));

            var client = new SmtpClient()
            {
                Port = options.Port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = options.Host,
                EnableSsl = true,
                Credentials = credentials
            };

            client.Send(mail);

            return Task.CompletedTask;
        }
    }
}

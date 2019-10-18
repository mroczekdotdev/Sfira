using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings settings;

        public EmailSender(IOptions<EmailSettings> settings)
        {
            this.settings = settings.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var credentials = new NetworkCredential(settings.Username, settings.Password);

            var mail = new MailMessage()
            {
                From = new MailAddress(settings.Username, settings.SenderName),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            mail.To.Add(new MailAddress(email));

            var client = new SmtpClient()
            {
                Port = settings.Port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = settings.Host,
                EnableSsl = true,
                Credentials = credentials
            };

            client.Send(mail);

            return Task.CompletedTask;
        }
    }

    public class EmailSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SenderName { get; set; }
    }
}

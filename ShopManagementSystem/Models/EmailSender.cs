using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;
using NuGet.Common;

namespace ShopManagementSystem.Models
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");
            var smtpServer = emailSettings["SMTPServer"];
            var smtpPort = int.Parse(emailSettings["SMTPPort"]);
            var smtpUsername = emailSettings["SMTPUsername"];
            var smtpPassword = emailSettings["SMTPPassword"];
            var senderName = emailSettings["SenderName"];

            var client = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderName),
                To = { new MailAddress(email) },
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            return client.SendMailAsync(mailMessage);
        }
    }
}

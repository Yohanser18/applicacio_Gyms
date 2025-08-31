using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using Microsoft.Extensions.Configuration;

namespace GYMS_TR_Utilidades
{
    public class EmailSender : IEmailSender
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;

        public EmailSender(IConfiguration config)
        {
            _smtpServer = config["EmailSettings:SmtpServer"];
            _smtpPort = int.Parse(config["EmailSettings:SmtpPort"]);
            _smtpUser = config["EmailSettings:SmtpUser"];
            _smtpPass = config["EmailSettings:SmtpPass"];
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            using var client = new SmtpClient(_smtpServer, _smtpPort)
            {
                Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpUser),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);
            await client.SendMailAsync(mailMessage);
        }
    }
}

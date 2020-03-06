using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;
using EmailSender.Configuration;
using MailKit.Net.Smtp;
using EmailSender.Helpers;

namespace EmailSender.Email
{
    public class EmailSenderImpl : IEmailSender
    {
        private readonly IEmailConfiguration _emailConfiguration;
        private readonly IEmailHelper _emailHelper;

        public EmailSenderImpl(IEmailConfiguration emailConfiguration, IEmailHelper emailHelper)
        {
            _emailConfiguration = emailConfiguration;
            _emailHelper = emailHelper;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailMessage = _emailHelper.CreateEmailMessage(email, subject, htmlMessage);
            var message = _emailHelper.CreateMimeMessage(emailMessage); 

            using (var emailClient = new SmtpClient())
            {
                emailClient.Connect("smtp.gmail.com", 465, true);
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);
                emailClient.Send(message);
                emailClient.Disconnect(true);
            }
        }
    }
}

using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Linq;
using System.Threading.Tasks;
using EmailSender.Configuration;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using System;

namespace EmailSender.Email
{
    public class EmailSenderImpl : IEmailSender
    {
        private readonly IEmailConfiguration _emailConfiguration;

        public EmailSenderImpl(IEmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailMessage = CreateEmailMessage(email, subject, htmlMessage);
            var message = CreateMimeMessage(emailMessage);

            using (var emailClient = new SmtpClient())
            {
                emailClient.Connect("smtp.gmail.com", 465, true);
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);
                emailClient.Send(message);
                emailClient.Disconnect(true);
            }
        }

        private MimeMessage CreateMimeMessage(EmailMessage emailMessage)
        {
            var message = new MimeMessage();
            message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
            message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
            message.Subject = emailMessage.Subject;
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = emailMessage.Content
            };
            return message;
        }

        private EmailMessage CreateEmailMessage(string email, string subject, string htmlMessage)
        {
            var emailMessage = new EmailMessage();
            emailMessage.ToAddresses = new List<EmailAddress> { new EmailAddress { Address = email } };
            emailMessage.FromAddresses = new List<EmailAddress> { new EmailAddress { Address = "airstorage.uk@gmail.com", Name = "AirStorage.Noreply" } };
            emailMessage.Subject = subject;
            emailMessage.Content = htmlMessage;
            return emailMessage;
        }
    }
}

using EmailSender.Email;
using EmailSender.Resources;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.Helpers.Impl
{
    public class EmailHelper : IEmailHelper
    {
        public EmailMessage CreateEmailMessage(string email, string subject, string htmlMessage)
        {
            var emailMessage = new EmailMessage();

            emailMessage.ToAddresses = new List<EmailAddress>();
            if (email != null)
                emailMessage.ToAddresses.Add(new EmailAddress() { Address = email });

            emailMessage.FromAddresses = new List<EmailAddress> { new EmailAddress { Address = EmailResources.airstorageFromAddress, Name = EmailResources.airstorageFromName } };

            if (subject != null)
                emailMessage.Subject = subject;
            if (htmlMessage != null)
                emailMessage.Content = htmlMessage;

            return emailMessage;
        }

        public MimeMessage CreateMimeMessage(EmailMessage emailMessage)
        {
            var message = new MimeMessage();

            if (emailMessage != null)
            {
                if (emailMessage.ToAddresses != null && emailMessage.FromAddresses != null)
                {
                    if (emailMessage.ToAddresses.Any(emailAddress => emailAddress.Address != null) && emailMessage.FromAddresses.Any(emailAddress => emailAddress.Address != null))
                    {
                        message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
                        message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
                    }
                }

                if (emailMessage.Subject != null)
                    message.Subject = emailMessage.Subject;

                if (emailMessage.Content != null)
                    message.Body = new TextPart(TextFormat.Html)
                    {
                        Text = emailMessage.Content
                    };
            }

            return message;
        }

        public bool EmailIsValid(string email)
        {
            try
            {
                MailAddress mail = new MailAddress(email);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

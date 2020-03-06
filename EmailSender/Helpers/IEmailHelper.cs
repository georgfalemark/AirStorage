using EmailSender.Email;
using MimeKit;

namespace EmailSender.Helpers
{
    public interface IEmailHelper
    {
        bool EmailIsValid(string email);
        EmailMessage CreateEmailMessage(string email, string subject, string htmlMessage);
        MimeMessage CreateMimeMessage(EmailMessage emailMessage);
    }
}

using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.Tests
{
    public class FakeSmtpClient : IDisposable
    {
        public bool Connected { get; set; }
        public bool MailSent { get; set; }

        public FakeSmtpClient()
        {
            Connected = false;
        }

        public void Connect()
        {
            Connected = true;
        }

        public void Send(MimeMessage message)
        {
            if (!message.From.Any() && !message.To.Any())
                MailSent = false;
            else if (string.IsNullOrEmpty(message.Subject))
                MailSent = false;
            else if (message.Body == null)
                MailSent = false;
            else
                MailSent = true;
        }

        public void Disconnect()
        {
            Connected = false;
            Dispose();
        }

        public void Dispose()
        {

        }
    }
}

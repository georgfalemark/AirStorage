using EmailSender.Email;
using EmailSender.Helpers;
using EmailSender.Helpers.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MimeKit;
using MimeKit.Text;
using System.Collections.Generic;
using System.Linq;

namespace EmailSender.Tests
{
    [TestClass]
    public class EmailSenderTest
    {
        private readonly IEmailHelper _emailHelper;

        public EmailSenderTest()
        {
            _emailHelper = new EmailHelper();
        }

        [TestMethod]
        [DataRow(null, null, null)]
        [DataRow("georg@falemark.se", "Das subject", null)]
        [DataRow("kalle@gmail.com", "testtest", "Das body Das body Das body Das body Das body Das body Das body Das body")]
        [DataRow(null, "Das subject 2", "<html> Test  Test  Test  Test  Test  Test  Test  Test </html>")]
        [DataRow("georg@falemark.se", "lorem ipsus", "<html><h1>Here is some Action you need to do</h1></html>")]
        [DataRow("olof@home.se", "lorem ipsus lorem ipsus lorem ipsus", "<html><b>Here is some Action you need to do</b></html>")]
        public void CreateEmailMessage_MultiInParameters_ShouldNotBeNull(string email, string subject, string htmlMessage)
        {
            var result = _emailHelper.CreateEmailMessage(email, subject, htmlMessage);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [DataRow(null, null, null)]
        [DataRow("georg@falemark.se", "Das subject", null)]
        [DataRow("kalle@gmail.com", "testtest", "Das body Das body Das body Das body Das body Das body Das body Das body")]
        [DataRow(null, "Das subject 2", "<html> Test  Test  Test  Test  Test  Test  Test  Test </html>")]
        [DataRow("georg@falemark.se", "lorem ipsus", "<html><h1>Here is some Action you need to do</h1></html>")]
        [DataRow("olof@home.se", "lorem ipsus lorem ipsus lorem ipsus", "<html><b>Here is some Action you need to do</b></html>")]
        public void CreateEmailMessage_MultiInParameters_SubjectAreEqual(string email, string subject, string htmlMessage)
        {
            var result = _emailHelper.CreateEmailMessage(email, subject, htmlMessage);
            var expected = new EmailMessage() { FromAddresses = new List<EmailAddress> { new EmailAddress { Address = "airstorage.uk@gmail.com", Name = "AirStorage" } }, ToAddresses = new List<EmailAddress> { new EmailAddress { Address = email } }, Subject = subject, Content = htmlMessage };
            Assert.AreEqual(result.Subject, expected.Subject);
        }

        static IEnumerable<object[]> EmailMessagesTestSetup
        {
            get
            {
                return new[]
                {
                new object[]
                {
                    new List<EmailMessage>
                    {
                        new EmailMessage
                        {
                            Subject = null,
                            Content = null,
                            ToAddresses = null,
                            FromAddresses = new List<EmailAddress>
                            {
                                new EmailAddress
                                {
                                    Address = "airstorage.uk@gmail.com",
                                    Name = null
                                }
                            }
                        },
                        new EmailMessage
                        {
                            Subject = "NOTICE YOU HAVE GOT A MAIL!",
                            Content = "",
                            ToAddresses = new List<EmailAddress>
                            {
                                new EmailAddress
                                {
                                    Address = "goto@gmail.com"
                                }
                            },
                            FromAddresses = new List<EmailAddress>
                            {
                                new EmailAddress
                                {
                                    Address = "airstorage.uk@gmail.com",
                                    Name = "AirStorage"
                                }
                            }
                        },
                        null,
                        new EmailMessage
                        {
                            Subject = "Mail mail",
                            Content = "Helll<> Helll<> Helll<> Helll<> Helll<> Helll<> Helll<>",
                            ToAddresses = new List<EmailAddress>
                            {
                                new EmailAddress
                                {
                                    Address = "kalle@gmail.com"
                                }
                            },
                            FromAddresses = new List<EmailAddress>
                            {
                                new EmailAddress
                                {
                                    Address = "airstorage.uk@gmail.com",
                                    Name = "AirStorage"
                                }
                            }
                        },
                        new EmailMessage
                        {
                            Subject = "NOTICE YOU HAVE GOT A MAIL!",
                            Content = "Das content das content",
                            ToAddresses = new List<EmailAddress>
                            {
                                new EmailAddress
                                {
                                    Address = "goto@gmail.com"
                                }
                            },
                            FromAddresses = new List<EmailAddress>
                            {
                                new EmailAddress
                                {
                                    Address = "airstorage.uk@gmail.com",
                                    Name = "AirStorage"
                                }
                            }
                        }
                    },
                }
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(EmailMessagesTestSetup))]
        public void CreateMimeMessage_MultipleParameters_ShouldNotBeNull(List<EmailMessage> emailMessages)
        {
            foreach (var emailMessage in emailMessages)
            {
                var result = _emailHelper.CreateMimeMessage(emailMessage);
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        [DynamicData(nameof(EmailMessagesTestSetup))]
        public void CreateMimeMessage_MultipleParameters_SubjectAreEqual(List<EmailMessage> emailMessages)
        {
            foreach (var emailMessage in emailMessages)
            {
                var result = _emailHelper.CreateMimeMessage(emailMessage);
                MimeMessage expected = new MimeMessage();
                if (emailMessage != null)
                    expected = new MimeMessage() { Subject = emailMessage.Subject != null ? emailMessage.Subject : string.Empty, Body = emailMessage.Content != null ? new TextPart(TextFormat.Html) { Text = emailMessage.Content } : null };
                Assert.AreEqual(result.Subject, expected.Subject);
            }
        }

        [TestMethod]
        public void EmailClientConnects_NoParameter_ShouldReturnTrue()
        {
            using (var emailClient = new FakeSmtpClient())
            {
                emailClient.Connect();
                Assert.IsTrue(emailClient.Connected);
            }
        }

        [TestMethod]
        public void EmailClientDisconnects_NoParameter_ConnectedShouldReturnFalse()
        {
            using (var emailClient = new FakeSmtpClient())
            {
                emailClient.Connect();
                emailClient.Disconnect();
                Assert.IsFalse(emailClient.Connected);
            }
        }

        [TestMethod]
        [DataRow("georg@falemark.se")]
        [DataRow("gggg@hhhh.com")]
        [DataRow("dsdsd.sdsds.sdsd@nisse.SE")]
        [DataRow("prutt.ksksksk@ddjdjdjd.sjjsjs.se")]
        [DataRow("sdfkjlfds@jfd23342.se")]
        [DataRow("ge2117fa-s@student.lu.se")]
        [DataRow("test@gmail.com")]
        [DataRow("d@jjdkfiwkdll.se")]
        [DataRow("d@jjdkfiwkdll.se")]
        public void EmailIsValid_MultipleValidParameters_ShouldReturnTrue(string email)
        {
            var isValid = _emailHelper.EmailIsValid(email);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [DataRow("dsdsd.sdsds.sdsdnisse.SE")]
        [DataRow("@prutt.kskskskddjdjdjd.sjjsjs.se")]
        [DataRow("s.dfkj.lfd.sjf.d23.342.se")]
        [DataRow("")]
        [DataRow(null)]
        [DataRow("ddksdlk23322132e")]
        [DataRow("12345")]
        public void EmailIsValid_MultipleInvalidParameters_ShouldReturnFalse(string email)
        {
            var isValid = _emailHelper.EmailIsValid(email);
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        [DataRow("nisse@gmail.com", "This is a test subject", "<html><h1>Here is some Action you need to do</h1></html>")]
        [DataRow("kallerapp@gmail.com", "lorem ipsum lorem ipsum lorem ipsum lorem ipsum", "////////////@@@@@@@///////")]
        [DataRow("johan@gmail.com", "This is another test subject", "<html><h1>Here is some Action you need to do</h1></html>")]
        [DataRow("georg@falemark.se", "4545454545454545454", "jhsfdsfhkdsfkdsfhkdh2324783293477437///DSDSDHF////")]
        [DataRow("test@hotmail.com", "123146544546", "jhsfdsfhkdsfkdsfhkdh2324783293477437///DSDSDHF////dfdfdsfds24233242424242424")]
        public void SendEmailTest_MultipleValidParameters_MailSentPropShouldReturnTrue(string email, string subject, string htmlMessage)
        {
            if (!_emailHelper.EmailIsValid(email))
                return;

            var emailMessage = _emailHelper.CreateEmailMessage(email, subject, htmlMessage);
            Assert.IsNotNull(emailMessage);
            var message = _emailHelper.CreateMimeMessage(emailMessage);
            Assert.IsNotNull(message);

            using (var emailClient = new FakeSmtpClient())
            {
                emailClient.Connect();
                emailClient.Send(message);
                emailClient.Disconnect();
                Assert.IsTrue(emailClient.MailSent);
            }
        }

        [TestMethod]
        [DataRow(null, null, null)]
        [DataRow("orvar_orvar_orvar", "", null)]
        [DataRow("nissgmail.com", null, null)]
        [DataRow("kallerapp@gmail.com", null, "")]
        [DataRow("test@hotmail.com", "1111", null)]
        [DataRow("test@hotmail.com", "test subject", null)]
        [DataRow("labanlabanhotmail.com", "jhsfdksfhd", null)]
        public void SendEmailTest_MultipleValidParameters_ShouldFailInSomeWay(string email, string subject, string htmlMessage)
        {
            if (!_emailHelper.EmailIsValid(email))
            {
                Assert.IsFalse(_emailHelper.EmailIsValid(email));
                return;
            }

            var emailMessage = _emailHelper.CreateEmailMessage(email, subject, htmlMessage);
            Assert.IsNotNull(emailMessage);
            var message = _emailHelper.CreateMimeMessage(emailMessage);
            Assert.IsNotNull(message);

            using (var emailClient = new FakeSmtpClient())
            {
                emailClient.Connect();
                emailClient.Send(message);
                Assert.IsFalse(emailClient.MailSent);
                emailClient.Disconnect();
            }
        }
    }
}

using System.Net;
using System.Net.Mail;
using System.Text;

namespace TestNinja.Mocking
{
    public interface IEmailManager
    {
        void EmailFile(string emailAddress, string emailBody, string filename, string subject);
    }
    
    public class EmailManager : IEmailManager
    {
        private readonly IEmailClient _client;

        public EmailManager(IEmailClient client)
        {
            _client = client;
        }
        
        public void EmailFile(string emailAddress, string emailBody, string filename, string subject)
        {
            // var smtpClient = new SmtpClient(SystemSettingsHelper.EmailSmtpHost)
            // {
            //     Port = SystemSettingsHelper.EmailPort,
            //     Credentials =
            //         new NetworkCredential(
            //             SystemSettingsHelper.EmailUsername,
            //             SystemSettingsHelper.EmailPassword)
            // };
            
            var from = new MailAddress(SystemSettingsHelper.EmailFromEmail, SystemSettingsHelper.EmailFromName, Encoding.UTF8);
            var to = new MailAddress(emailAddress);

            var message = new MailMessage(from, to)
            {
                Subject = subject,
                SubjectEncoding = Encoding.UTF8,
                Body = emailBody,
                BodyEncoding = Encoding.UTF8
            };

            message.Attachments.Add(new Attachment(filename));
            _client.Send(message);
            message.Dispose();
        }
    }

    public interface IEmailClient
    {
        void Send(MailMessage message);
    }

    public class EmailClient : IEmailClient
    {
        private readonly SmtpClient _smtpClient;

        public EmailClient()
        {
            _smtpClient = new SmtpClient(SystemSettingsHelper.EmailSmtpHost)
            {
                Port = SystemSettingsHelper.EmailPort,
                Credentials =
                    new NetworkCredential(
                        SystemSettingsHelper.EmailUsername,
                        SystemSettingsHelper.EmailPassword)
            };
        }

        public void Send(MailMessage message)
        {
            _smtpClient.Send(message);
        }
    }
}
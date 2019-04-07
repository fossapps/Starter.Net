using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Starter.Net.Api.Configs;

namespace Starter.Net.Api.Mails
{
    public class SmtpMailService : IMailService
    {
        private readonly SmtpClient _smtpClient;
        public SmtpMailService(IOptions<Mail> mailConfig)
        {
            var smtpConfig = mailConfig.Value.Smtp;
            var client = new SmtpClient(smtpConfig.Host, smtpConfig.Port)
            {
                Credentials = new NetworkCredential(smtpConfig.User, smtpConfig.Password)
            };
            _smtpClient = client;
        }

        public void Send(MailMessage mailMessage)
        {
            _smtpClient.SendAsync(mailMessage, null);
        }
    }
}

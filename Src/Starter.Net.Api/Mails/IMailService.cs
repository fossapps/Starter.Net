using System.Net.Mail;

namespace Starter.Net.Api.Mails
{
    public interface IMailService
    {
        void Send(MailMessage mailMessage);
    }
}

using System.Net.Mail;
using Starter.Net.Api.Configs;

namespace Starter.Net.Api.Mails.Content
{
    public class ActivationEmail : BaseEmail
    {
        public ActivationEmail(Mail mail) : base(mail)
        {
        }

        public MailMessage Build(string activationUrl, MailAddress recipient)
        {
            var template = GetTemplate();
            var content = template.Replace("{{ActivationUrl}}", activationUrl);
            var messageBuilder = new MailMessageBuilder();
            var mailMessageCollection = new MailAddressCollection() {recipient};
            return messageBuilder
                .From(new MailAddress(MailConfig.DefaultSender.From, MailConfig.DefaultSender.Name))
                .WithSender(new MailAddress(MailConfig.DefaultSender.From, MailConfig.DefaultSender.Name))
                .WithSubject("Activate your Email")
                .WithHtmlBody(content)
                .AddRecipients(mailMessageCollection)
                .Build();
        }
    }
}

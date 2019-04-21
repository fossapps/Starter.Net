using System.Net.Mail;
using Starter.Net.Api.Configs;

namespace Starter.Net.Api.Mails.Content
{
    public class InvitationEmail : BaseEmail
    {
        public InvitationEmail(Mail mail) : base(mail)
        {
        }

        public MailMessage Build(MailAddress recipient, string userName, string targetLink)
        {
            var template = GetTemplate();
            var content = template
                .Replace("{{User}}", userName)
                .Replace("{{SignUpUrl}}", targetLink);
            var messageBuilder = new MailMessageBuilder();
            return messageBuilder
                .From(new MailAddress(MailConfig.DefaultSender.From, MailConfig.DefaultSender.Name))
                .WithSender(new MailAddress(MailConfig.DefaultSender.From, MailConfig.DefaultSender.Name))
                .WithSubject($"You are invited by {userName}")
                .WithHtmlBody(content)
                .AddRecipient(recipient)
                .Build();
        }
    }
}

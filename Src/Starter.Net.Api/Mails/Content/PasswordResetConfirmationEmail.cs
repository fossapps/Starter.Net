using System.Net.Mail;
using Starter.Net.Api.Configs;

namespace Starter.Net.Api.Mails.Content
{
    public class PasswordResetConfirmationEmail : BaseEmail
    {
        public PasswordResetConfirmationEmail(Mail mail) : base(mail)
        {
        }

        public MailMessage Build(MailAddress recipient)
        {
            var template = GetTemplate();
            var messageBuilder = new MailMessageBuilder();
            var mailMessageCollection = new MailAddressCollection() {recipient};
            return messageBuilder
                .From(new MailAddress(MailConfig.DefaultSender.From, MailConfig.DefaultSender.Name))
                .WithSender(new MailAddress(MailConfig.DefaultSender.From, MailConfig.DefaultSender.Name))
                .WithSubject("Password Reset Successful")
                .WithHtmlBody(template)
                .AddRecipients(mailMessageCollection)
                .Build();
        }
    }
}

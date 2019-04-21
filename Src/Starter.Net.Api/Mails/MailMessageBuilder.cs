using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Starter.Net.Api.Mails
{
    public class MailMessageBuilder
    {
        private readonly MailMessage _message;

        public MailMessageBuilder(MailMessage message = null)
        {
            if (message == null)
            {
                message = new MailMessage();
            }
            _message = message;
        }

        public MailMessageBuilder AddRecipient(MailAddress address)
        {
            _message.To.Add(address);
            return this;
        }

        public MailMessageBuilder AddRecipients(MailAddressCollection addressCollection)
        {
            foreach (var mailAddress in addressCollection)
            {
                _message.To.Add(mailAddress);
            }

            return this;
        }

        public MailMessageBuilder AddCarbonCopyRecipients(MailAddressCollection addressCollection)
        {
            foreach (var mailAddress in addressCollection)
            {
                _message.CC.Add(mailAddress);
            }

            return this;
        }

        public MailMessageBuilder AddBlankCarbonCopyRecipients(MailAddressCollection addressCollection)
        {
            foreach (var mailAddress in addressCollection)
            {
                _message.Bcc.Add(mailAddress);
            }

            return this;
        }

        public MailMessageBuilder WithSubject(string subject)
        {
            _message.Subject = subject;
            return this;
        }

        public MailMessageBuilder WithSender(MailAddress address)
        {
            _message.Sender = address;
            return this;
        }

        public MailMessageBuilder From(MailAddress address)
        {
            _message.From = address;
            return this;
        }

        public MailMessageBuilder WithPlainTextBody(string body)
        {
            _message.Body = body;
            return this;
        }

        public MailMessageBuilder WithHtmlBody(string body)
        {
            var view = AlternateView
                .CreateAlternateViewFromString(body, Encoding.UTF8, MediaTypeNames.Text.Html);
            _message.AlternateViews.Add(view);
            return this;
        }

        public MailMessage Build()
        {
            return _message;
        }
    }
}

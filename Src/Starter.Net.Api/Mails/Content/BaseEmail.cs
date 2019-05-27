using System.IO;
using Starter.Net.Api.Configs;

namespace Starter.Net.Api.Mails.Content
{
    public abstract class BaseEmail
    {
        protected readonly Mail MailConfig;
        protected BaseEmail(Mail mail)
        {
            MailConfig = mail;
        }

        protected static string GetTemplateByName(string name)
        {
            return File.ReadAllText($"./Mails/Content/Templates/{name}.html");
        }

        protected string GetTemplate()
        {
            return GetTemplateByName(this.GetType().Name);
        }
    }
}

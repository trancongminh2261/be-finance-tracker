using MonaDotNetTemplate.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MonaDotNetTemplate.Services.Interface.Notification
{
    public interface IEmailConfigurationService
    {
        Task<EmailSendConfigure> GetEmailConfig();
        EmailContent GetEmailContent();
        EmailContent GetEmailContent(string template, IDictionary<string, string> param);
        Task SendAsync(string subject, string body, string[] Tos);
        Task SendAsync(string subject, string body, string[] Tos, string[] CCs);
        Task SendAsync(string subject, string body, string[] Tos, string[] CCs, string[] BCCs);
        Task SendAsync(string subject, string[] Tos, EmailContent emailContent);
        Task SendAsync(string subject, string[] Tos, string[] CCs,  EmailContent emailContent);
        Task SendAsync(string subject, string[] Tos, string[] CCs, string[] BCCs, EmailContent emailContent);
    }
}

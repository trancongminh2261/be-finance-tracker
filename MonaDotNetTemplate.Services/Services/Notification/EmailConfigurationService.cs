using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MonaDotNetTemplate.Services.Interface.Notification;
using MonaDotNetTemplate.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace NhapHangV2.Service.Services.Configurations
{
    public class EmailConfigurationService: IEmailConfigurationService
    {
        protected IConfiguration _configuration;

        public EmailConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        /// <summary>
        /// Lấy thông tin cấu hình Email
        /// </summary>
        /// <returns></returns>
        public async Task<EmailSendConfigure> GetEmailConfig()
        {
            EmailSendConfigure emailSendConfigure = new EmailSendConfigure();
            var configuration = _configuration.GetSection("EmailConfig").Get<EmailSendConfigure>();
            configuration.Priority = MailPriority.Normal;
            configuration.Subject = string.Empty;
            configuration.TOs = new List<string>();
            return configuration;
        }

        /// <summary>
        /// Lấy thông tin nội dung email
        /// </summary>
        /// <returns></returns>
        public EmailContent GetEmailContent()
        {
            return new EmailContent()
            {
                IsHtml = true,
                Content = string.Empty
            };
        }


        public MailMessage ConstructEmailMessage(EmailSendConfigure emailConfig, EmailContent content)
        {
            MailMessage msg = new MailMessage();
            if (emailConfig.TOs != null)
            {
                foreach (string to in emailConfig.TOs)
                {
                    if (!string.IsNullOrEmpty(to))
                    {
                        msg.To.Add(to);
                    }
                }
            }
            

            if (emailConfig.CCs != null)
            {
                foreach (string cc in emailConfig.CCs)
                {
                    if (!string.IsNullOrEmpty(cc))
                    {
                        msg.CC.Add(cc);
                    }
                }
            }
            if (emailConfig.BCCs != null)

                foreach (string bcc in emailConfig.BCCs)
                {
                    if (!string.IsNullOrEmpty(bcc))
                    {
                        msg.Bcc.Add(bcc);
                    }
                }
            msg.From = new MailAddress(emailConfig.FromEmail,
                                           emailConfig.FromDisplayName,
                                           Encoding.UTF8);
            msg.IsBodyHtml = content.IsHtml;
            msg.Body = content.Content;
            msg.Priority = emailConfig.Priority;
            msg.Subject = emailConfig.Subject;
            msg.BodyEncoding = Encoding.UTF8;
            msg.SubjectEncoding = Encoding.UTF8;

            if (content.Attachments != null && content.Attachments.Count > 0)
            {
                foreach (var attachment in content.Attachments)
                {
                    msg.Attachments.Add(attachment);
                }
            }
            return msg;
        }

        /// <summary>
        /// Send the email using the SMTP server 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="emailConfig"></param>
        public async Task SendAsync(MailMessage message, EmailSendConfigure emailConfig)
        {
            await Task.Run(() =>
            {
                SmtpClient client = new SmtpClient
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(
                                  emailConfig.ClientCredentialUserName,
                                  emailConfig.ClientCredentialPassword),
                    Host = emailConfig.SmtpServer,
                    Port = emailConfig.Port,
                    EnableSsl = emailConfig.EnableSsl,
                };
                Console.WriteLine("------------------------------------ Email:" + emailConfig.FromEmail);
                Console.WriteLine("------------------------------------ ClientCredentialUserName:" + emailConfig.ClientCredentialUserName);
                Console.WriteLine("------------------------------------ Password:" + emailConfig.ClientCredentialPassword);
                try
                {
                    client.Send(message);
                }
                catch (SmtpFailedRecipientsException ex)
                {
                    throw new Exception("SmtpFailedRecipientsException:" + ex.StackTrace);
                }
                catch (Exception e)
                {
                    throw new Exception("ExceptionMAIL:" + e.StackTrace);
                }
                finally
                {
                    message.Dispose();
                }
            });

        }

        public async Task SendAsync(string subject, string body, string[] Tos)
        {
            await SendAsync(subject, body, Tos, null, null);
        }


        public async Task SendAsync(string subject, string body, string[] Tos, string[] CCs)
        {
            await SendAsync(subject, body, Tos, CCs, null);
        }


        public async Task SendAsync(string subject, string body, string[] Tos, string[] CCs, string[] BCCs)
        {
            EmailSendConfigure emailConfig = await GetEmailConfig();
            EmailContent content = GetEmailContent();
            emailConfig.Subject = subject;
            emailConfig.TOs = Tos;
            emailConfig.CCs = CCs;
            emailConfig.BCCs = BCCs;
            content.Content = body;
            MailMessage msg = ConstructEmailMessage(emailConfig, content);
            await SendAsync(msg, emailConfig);
        }

        public async Task SendAsync(string subject, string[] Tos, string[] CCs, string[] BCCs, EmailContent emailContent)
        {
            EmailSendConfigure emailConfig = await GetEmailConfig();
            EmailContent content = GetEmailContent();
            emailConfig.Subject = subject;
            emailConfig.TOs = Tos;
            emailConfig.CCs = CCs;
            emailConfig.BCCs = BCCs;
            content = emailContent;
            MailMessage msg = ConstructEmailMessage(emailConfig, content);
            await SendAsync(msg, emailConfig);
        }

        public EmailContent GetEmailContent(string template, IDictionary<string, string> param)
        {
            var content = File.ReadAllText(template);
            foreach(var item in param.Keys)
            {
                content.Replace(item, param[item]);
            }
            return new()
            {
                Content = content,
                IsHtml = true
            };
        }

        public async Task SendAsync(string subject, string[] Tos, EmailContent emailContent)
        {
            await SendAsync(subject, Tos, null, null, emailContent);
        }

        public async Task SendAsync(string subject, string[] Tos, string[] CCs, EmailContent emailContent)
        {
            await SendAsync(subject, Tos, CCs, null, emailContent);
        }
    }
}

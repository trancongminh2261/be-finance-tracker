using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace MonaDotNetTemplate.Utilities
{
    /// <summary>
    /// Gửi mail
    /// </summary>
    public class EmailSendConfigure
    {
        public string SmtpServer { set; get; }
        public IList<string> TOs { get; set; }
        public IList<string> CCs { get; set; }
        public string FromEmail { get; set; }
        public bool EnableSsl { get; set; }
        public int Port { get; set; }
        public string FromDisplayName { get; set; }
        public string Subject { get; set; }
        public MailPriority Priority { get; set; }
        public string ClientCredentialUserName { get; set; }
        public string ClientCredentialPassword { get; set; }
        public IList<string> BCCs { get; set; }

        public EmailSendConfigure()
        {
            TOs = new List<string>();
            CCs = new List<string>();
            BCCs = new List<string>();
        }
    }
}

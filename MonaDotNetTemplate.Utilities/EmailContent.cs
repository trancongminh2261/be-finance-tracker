﻿using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace MonaDotNetTemplate.Utilities
{
    public class EmailContent
    {
        public EmailContent()
        {
            Attachments = new List<Attachment>();
        }

        public bool IsHtml { get; set; }
        public string Content { get; set; }
        public IList<Attachment> Attachments { get; set; }
    }
}

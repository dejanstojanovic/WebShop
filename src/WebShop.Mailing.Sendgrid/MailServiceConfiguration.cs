using System;
using System.Collections.Generic;
using System.Text;

namespace WebShop.Mailing.Sendgrid
{
    public class MailServiceConfiguration
    {
        public String ApiKey { get; set; }
        public String DefaultSender { get; set; }
    }
}

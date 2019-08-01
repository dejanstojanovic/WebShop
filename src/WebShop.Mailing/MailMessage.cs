using System;
using System.Collections.Generic;
using System.Text;

namespace WebShop.Mailing
{
    public class MailMessage
    {
        public String From { get; set; }
        public IEnumerable<String> To { get; set; }
        public String Subject { get; set; }
        public String Body { get; set; }

        public MailMessage()
        {
            this.To = new List<String>();
        }
    }
}

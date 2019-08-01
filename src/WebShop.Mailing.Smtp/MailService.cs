using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Mailing.Smtp
{
    public class MailService : IMailService
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task SendAsync(MailMessage message)
        {
            throw new NotImplementedException();
        }
    }
}

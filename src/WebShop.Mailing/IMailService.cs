using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Mailing
{
    public interface IMailService:IDisposable
    {
        Task SendAsync(MailMessage message);
    }
}

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebShop.Mailing.Sendgrid.Extensions
{
    public static class SendgridExtensions
    {

        public static void AddSendgridMailingService(this IServiceCollection services)
        {
            services.AddSingleton<IMailService, MailService>();
        }

    }
}

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebShop.Mailing.Sendgrid.Extensions
{
    public static class SendgridExtensions
    {

        public static void AddSendgridMailer(this IServiceCollection services)
        {
            services.AddSingleton<IMailer, Mailer>();
        }

    }
}

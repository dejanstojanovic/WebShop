using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebShop.Messaging.ServiceBus.Extensions
{
    public static class ServiceBusExtensions
    {
        public static void AddServiceBusPublisher<TModel>(this IServiceCollection services) where TModel : IMessage
        {
            services.AddSingleton<IMessagePublisher<TModel>, MessagePublisher<TModel>>();
        }

        public static void AddServiceBusSubsciber<TModel>(this IServiceCollection services) where TModel :IMessage
        {
            services.AddSingleton<IMessageSubscriber<TModel>, MessageSubscriber<TModel>>();
        }

        public static void UseServiceBusPublisher<TModel>(this IApplicationBuilder app) where TModel : IMessage
        {
            app.ApplicationServices.GetService<IMessagePublisher<TModel>>();
        }

        public static void UseServiceBusSubscriber<TModel>(this IApplicationBuilder app) where TModel : IMessage
        {
            app.ApplicationServices.GetService<IMessageSubscriber<TModel>>();
        }
    }
}

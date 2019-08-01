using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WebShop.Messaging.Extensions
{
    public static class HandlerExtensions
    {

        /// <summary>
        /// Registers resolvers for command and query dispatchers
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly">Assmbly containing ICommandHandler and IQueryHandler interface implementations to be used</param>
        public static void AddMessagingServices(this IServiceCollection services, Assembly assembly = null)
        {
            if (assembly == null)
            {
                assembly = Assembly.GetCallingAssembly();
            }

            services.AddHandlerServices(typeof(ICommandHandler<>),assembly);
            services.AddHandlerServices(typeof(IEventHandler<>), assembly);

            services.AddHandlerServices(typeof(IQueryHandler<,>),assembly);
            
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            services.AddScoped<IQueryDispatcher, QueryDispatcher>();

        }

        private static void AddHandlerServices(this IServiceCollection services, Type handlerInterface, Assembly assembly)
        {
            var handlers = assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface)
            );

            foreach (var handler in handlers)
            {
                var interfaces = handler.GetInterfaces().Where(i => i.IsGenericType &&
                       (i.GenericTypeArguments.Any(a => a.GetInterfaces().Any(m=>m == typeof(IMessage)))  ||
                        i.GetGenericTypeDefinition() == handlerInterface
                       ));

                foreach (var @interface  in interfaces)
                {
                    services.AddTransient(@interface, handler);
                }

            }
        }
    }
}

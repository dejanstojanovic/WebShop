using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebShop.Messaging;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebShop.Messaging.ServiceBus
{
    public class MessageSubscriber<TMessage> : BusClient<TMessage>, IMessageSubscriber<TMessage> where TMessage : IMessage
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly IQueueClient _queueClient;
        private ILogger<MessageSubscriber<TMessage>> _logger;
        public MessageSubscriber(IConfiguration configuration,
                                 IServiceProvider serviceProvider,
                                 ILogger<MessageSubscriber<TMessage>> logger):base(configuration)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;

            _queueClient = new QueueClient(configuration.GetConnectionString("ServiceBus"),
                !String.IsNullOrWhiteSpace(QueueName) ? QueueName : typeof(TMessage).Name.ToLowerInvariant());

            _queueClient.RegisterMessageHandler(
                   handler: ProcessMessagesAsync,
                   messageHandlerOptions: new MessageHandlerOptions(ExceptionReceivedHandler)
                   {
                       MaxConcurrentCalls = 1,
                       AutoComplete = false
                   });

        }


        async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {

            try
            {
                await this.HandleAsync(JsonConvert.DeserializeObject<TMessage>(Encoding.UTF8.GetString(message.Body)), Guid.Parse(message.CorrelationId));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Azure ServiceBus subscriber failed processing message {typeof(TMessage).FullName}");
                await _queueClient.AbandonAsync(message.SystemProperties.LockToken);
            }

            await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        async Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            _logger.LogError(exceptionReceivedEventArgs.Exception,"Failed receiving message");
            await Task.CompletedTask;
        }
        

        public async Task HandleAsync(TMessage message, Guid correlationId)
        {
            using (var handler = _serviceProvider.GetService(typeof(IMessageHandler<TMessage>)) as IMessageHandler<TMessage>)
            {
                await handler.HandleAsync(message);
            }
        }

        public void Dispose()
        {
            
        }
    }
}

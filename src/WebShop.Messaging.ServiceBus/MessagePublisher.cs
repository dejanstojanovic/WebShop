using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using WebShop.Messaging;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.ServiceBus.Core;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace WebShop.Messaging.ServiceBus
{
    public class MessagePublisher<TMessage> : BusClient<TMessage>, IMessagePublisher<TMessage> where TMessage : IMessage
    {

        private readonly ISenderClient _senderClient;
        private ILogger<MessagePublisher<TMessage>> _logger;
        public MessagePublisher(
             IConfiguration configuration,
             ILogger<MessagePublisher<TMessage>> logger):base(configuration)
        {           
            _logger = logger;
                _senderClient = new QueueClient(
                    _configuration.GetConnectionString("ServiceBus"),
                    !String.IsNullOrWhiteSpace(QueueName) ? QueueName : typeof(TMessage).Name.ToLowerInvariant());
        }

        public void Dispose()
        {
            
        }

        public async Task Publish(TMessage message, Guid correlationId)
        {
            var busMessage = new Message(
                    Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message))
                    );
            await _senderClient.SendAsync(
                busMessage
                );
        }
    }
}

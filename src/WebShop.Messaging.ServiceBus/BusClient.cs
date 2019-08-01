using Microsoft.Azure.Management.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Messaging.ServiceBus
{
    public abstract class BusClient<TMessage> where TMessage : IMessage
    {
        private bool _topologyRestored;
        protected readonly IConfiguration _configuration;
        public String QueueName
        {
            get
            {
                return (typeof(TMessage).GetCustomAttributes(typeof(MessageQueueAttribute), true).FirstOrDefault() as MessageQueueAttribute)?.Name?.ToLower();
            }
        }

        public BusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            _topologyRestored = false;
        }

        protected async Task RestoreTopology()
        {
            if (!_topologyRestored)
            {
                ManagementClient managementClient = new ManagementClient(_configuration.GetConnectionString("ServiceBus"));
                if (!String.IsNullOrEmpty(this.QueueName) && await managementClient.TopicExistsAsync(this.QueueName))
                {
                    await managementClient.CreateQueueAsync(this.QueueName);
                }
                _topologyRestored = true;
            }
            
        }
    }
}

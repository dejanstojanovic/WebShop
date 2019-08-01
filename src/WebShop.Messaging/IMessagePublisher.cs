using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebShop.Messaging;

namespace WebShop.Messaging
{
    public interface IMessagePublisher<TMessage>:IDisposable where TMessage :IMessage
    {
        Task Publish(TMessage message);
    }
}

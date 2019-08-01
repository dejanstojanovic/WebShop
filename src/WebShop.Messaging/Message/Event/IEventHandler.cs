using System;
using System.Collections.Generic;
using System.Text;
using WebShop.Messaging;
using System.Threading;
using System.Threading.Tasks;

namespace WebShop.Messaging
{
    public interface IEventHandler<TEvent>:IMessageHandler<TEvent> where TEvent : IEvent
    {
    }
}

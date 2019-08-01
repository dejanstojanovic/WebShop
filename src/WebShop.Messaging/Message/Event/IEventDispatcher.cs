using System;
using System.Collections.Generic;
using System.Text;
using GetBee.Messaging;
using System.Threading;

namespace GetBee.Messaging
{
    public interface IEventDispatcher<TEvent> where TEvent:IEvent
    {
        Task<bool> HandleAsync(TEvent @event);
    }
}

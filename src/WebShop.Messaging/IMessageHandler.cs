using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Messaging
{
    public interface IMessageHandler<TMessage>:IDisposable where TMessage : IMessage
    {
        Task HandleAsync(TMessage message);
    }
}

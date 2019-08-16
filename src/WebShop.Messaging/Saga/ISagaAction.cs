using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Messaging.Saga
{
   public interface ISagaAction<TMessage> where TMessage : IMessage
    {
        Task HandleAsync(TMessage message);
        Task CompensateAsync(TMessage message);
    }
}

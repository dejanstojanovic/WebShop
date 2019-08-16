using System;
using System.Threading.Tasks;

namespace WebShop.Messaging.Saga
{
    public interface ISaga<TState> where TState:ISagaState
    {
        Guid Id { get; }
        Task CompleteAsync();
    }
}

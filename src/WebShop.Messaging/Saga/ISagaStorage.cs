using System;
using System.Threading.Tasks;

namespace WebShop.Messaging.Saga
{
    public interface ISagaStorage<TState> where TState:ISagaState
    {
        Task<TState> GetStateAsync(Guid sagaId);
        Task SetStateAsync(Guid sagaId, TState state);
    }
}

using System;
using System.Threading.Tasks;

namespace WebShop.Messaging.Saga
{
    public interface ISagaStorage
    {
        Task<ISagaState> GetStateAsync(Guid sagaId);
        Task SetStateAsync(Guid sagaId, ISagaState state);
    }
}

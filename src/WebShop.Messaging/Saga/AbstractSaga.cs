using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Messaging.Saga
{
    public abstract class AbstractSaga<TState> : ISaga<TState> where TState : ISagaState
    {
        public Guid Id { get; }

        readonly ISagaStorage<TState> _sagaStorage;

        protected AbstractSaga(Guid id, ISagaStorage<TState> storage)
        {
            Id = id;
            _sagaStorage = storage;
        }

        public async Task CompleteAsync()
        {
            await _sagaStorage.DeleteAsync(this.Id);
        }
    }
}

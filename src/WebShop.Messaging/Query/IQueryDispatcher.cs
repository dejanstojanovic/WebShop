using WebShop.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Messaging
{
    public interface IQueryDispatcher
    {
        Task<TResult> HandleAsync<TQuery,TResult>(TQuery query) where TQuery : IQuery<TResult>;
    }
}

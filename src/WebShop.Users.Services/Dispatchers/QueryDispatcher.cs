using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebShop.Messaging;

namespace WebShop.Users.Services
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private IServiceProvider _serviceProvider;

        public QueryDispatcher(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public async Task<TResult> HandleAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
        {
            var service = this._serviceProvider.GetService(typeof(IQueryHandler<TQuery,TResult>)) as IQueryHandler<TQuery,TResult>;
            return await service.HandleAsync(query);
        }
    }
}

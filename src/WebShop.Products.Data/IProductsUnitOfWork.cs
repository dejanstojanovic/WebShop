using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebShop.Common.Database;
using WebShop.Products.Data.Repositories;

namespace WebShop.Products.Data
{
    public interface IProductsUnitOfWork : IDisposable
    {
        /// <summary>
        /// Products repository instance
        /// </summary>
        IProductsRepository Products { get; }

        /// <summary>
        /// Update an existing entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Update<T>(T entity) where T : GenericEntity;

        /// <summary>
        /// Save changes
        /// </summary>
        /// <returns></returns>
        int Save();

        /// <summary>
        /// Save change asynchronously
        /// </summary>
        /// <returns></returns>
        Task<int> SaveAsync();

        /// <summary>
        /// Creates trnasaction of the dbContext of the unitOfWork instance
        /// </summary>
        IDatabaseTransaction BeginTransaction { get; }
    }

}

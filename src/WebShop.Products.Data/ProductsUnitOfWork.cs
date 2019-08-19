using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebShop.Common.Database;
using WebShop.Common.Exceptions;
using WebShop.Products.Data.Repositories;

namespace WebShop.Products.Data
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ProductsUnitOfWork : IProductsUnitOfWork
    {
        public IProductsRepository Products { get; private set; }

        public IDatabaseTransaction BeginTransaction => new EntityDatabaseTransaction(_dbContext);

        private bool _disposing = false;
        private readonly ProductsDbContext _dbContext;
        public ProductsUnitOfWork(ProductsDbContext dbContext)
        {
            _dbContext = dbContext;
            this.Products = new ProductsRepository(dbContext);
        }


        public void Dispose()
        {
            if (!_disposing)
            {
                _disposing = true;
                this._dbContext.Dispose();
            }
        }

        public int Save()
        {
            try
            {
                return this._dbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DataConcurrencyException(ex.Message, ex);
            }
        }

        public async Task<int> SaveAsync()
        {
            try
            {
                return await this._dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DataConcurrencyException(ex.Message, ex);
            }
            
        }

        public void Update<T>(T entity) where T : GenericEntity
        {
            _dbContext.Set<T>().Update(entity);
        }
    }
}

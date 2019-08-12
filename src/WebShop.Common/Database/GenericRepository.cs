using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebShop.Common.Database
{

    /// <summary>
    /// Generic abstract repository
    /// </summary>
    /// <typeparam name="T">Type of entities repository is responsible for</typeparam>
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : GenericEntity
    {

        DbContext dbContext;
        bool disposing;

        /// <summary>
        /// Default parameterless respository constructor
        /// </summary>
        public DbContext DbContext
        {
            get
            {
                return this.dbContext;
            }
        }

        /// <summary>
        /// Repository constructor
        /// </summary>
        /// <param name="dbContext">Database context to use for the repository</param>
        public GenericRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Dispose the repository and inner components (dbContext)
        /// </summary>
        public virtual void Dispose()
        {
            if (!this.disposing)
            {
                this.dbContext.Dispose();
                this.disposing = true;
            }
        }

        /// <summary>
        /// Get entity of type T aynchronously
        /// </summary>
        /// <param name="id">Entity id GUID instance</param>
        /// <returns></returns>
        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await this.dbContext.Set<T>().FindAsync(id);
        }

        /// <summary>
        /// Get entity of type T aynchronously
        /// </summary>
        /// <param name="id">Entity id GUID string value</param>
        /// <returns></returns>
        public virtual async Task<T> GetByIdAsync(string id)
        {
            return await GetByIdAsync(Guid.Parse(id));
        }

        /// <summary>
        /// Get IQueryable for the specific type T in the dbConext
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> Get()
        {
            return this.dbContext.Set<T>().AsNoTracking();
        }

        /// <summary>
        /// Inserts new entity of type T asynchronously
        /// </summary>
        /// <param name="item">Entity instance</param>
        /// <returns></returns>
        public virtual async Task<Guid> InsertAsync(T item)
        {
            await this.dbContext.Set<T>().AddAsync(item);
            return item.Id;
        }

        /// <summary>
        /// Retuns IQueryable with appliex expression filter
        /// </summary>
        /// <param name="expression">FIltering expression</param>
        /// <returns></returns>
        public virtual IQueryable<T> Find(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return this.dbContext.Set<T>().AsNoTracking().Where(expression);
        }

        /// <summary>
        /// Remove enity with specific id
        /// </summary>
        /// <param name="id">Entity unique identifier</param>
        public void Remove(Guid id)
        {
            var item = this.dbContext.Set<T>().FirstOrDefault(e => e.Id == id);
            if (item != null)
            {
                this.dbContext.Set<T>().Remove(item);
            }
        }

        /// <summary>
        /// Remove multiple enitties from the unerlying dbConext
        /// </summary>
        /// <param name="ids">Collection of GUID instances to remove from dbContext</param>
        public void RemoveRange(IEnumerable<Guid> ids)
        {
            var items = this.dbContext.Set<T>().Where(e => ids.Contains(e.Id));
            if (items != null && items.Any())
            {
                this.dbContext.Set<T>().RemoveRange(items);
            }
        }

        /// <summary>
        /// Add multiple entities of the same type to the dbConext
        /// </summary>
        /// <param name="items">Entity class instances</param>
        /// <returns></returns>
        public async Task InsertRangeAsync(IEnumerable<T> items)
        {
            await this.dbContext.Set<T>().AddRangeAsync(items);
        }


    }
}

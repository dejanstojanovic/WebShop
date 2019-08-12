using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebShop.Common.Database
{

    /// <summary>
    /// Repository base interface
    /// </summary>
    /// <typeparam name="T">Type of entities repository is responsible for</typeparam>
    public interface IGenericRepository<T> : IDisposable where T : GenericEntity
    {

        /// <summary>
        /// Get entity for specific id asynchronously
        /// </summary>
        /// <param name="id">Entity id GUID value</param>
        /// <returns></returns>
        Task<T> GetByIdAsync(Guid id);

        /// <summary>
        /// Get entity for specific id asynchronously
        /// </summary>
        /// <param name="id">String value of id GUID</param>
        /// <returns></returns>
        Task<T> GetByIdAsync(String id);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IQueryable<T> Get();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IQueryable<T> Find(Expression<Func<T, bool>> expression);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<Guid> InsertAsync(T item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        Task InsertRangeAsync(IEnumerable<T> items);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        void Remove(Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        void RemoveRange(IEnumerable<Guid> ids);



    }

}

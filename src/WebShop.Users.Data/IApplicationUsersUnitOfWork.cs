using WebShop.Users.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Users.Data
{
    public interface IApplicationUsersUnitOfWork:IDisposable
    {
        IApplicationUserRepository ApplicationUsers { get; }

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

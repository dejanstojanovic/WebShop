using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebShop.Common.Database
{
    /// <summary>
    /// Database transaction conrete implementation
    /// </summary>
    public class EntityDatabaseTransaction : IDatabaseTransaction
    {
        private IDbContextTransaction transaction;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Database context to apply transaction to</param>
        public EntityDatabaseTransaction(DbContext context)
        {
            transaction = context.Database.BeginTransaction();
        }

        /// <summary>
        /// Commit dbConext changes done in transaction
        /// </summary>
        public void Commit()
        {
            transaction.Commit();
        }

        /// <summary>
        /// Rollback dbConext changes done in transaction
        /// </summary>
        public void Rollback()
        {
            transaction.Rollback();
        }

        /// <summary>
        /// Closes and disposes current transaction
        /// </summary>
        public void Dispose()
        {
            transaction.Dispose();
        }
    }


}

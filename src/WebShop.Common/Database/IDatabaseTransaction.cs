﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WebShop.Common.Database
{
    /// <summary>
    /// Database transaction interface
    /// </summary>
    public interface IDatabaseTransaction : IDisposable
    {
        /// <summary>
        /// Commits the transaction changes
        /// </summary>
        void Commit();

        /// <summary>
        /// Rolls back transaction changes
        /// </summary>
        void Rollback();
    }
}

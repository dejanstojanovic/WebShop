using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebShop.Common.Extensions
{
    /// <summary>
    /// Unit testing extension methods to help mocking EntityFramework
    /// </summary>
    public static class UnitTesting
    {
        public static Mock<DbSet<TEntity>> GetDbSetMock<TEntity>(this IList<TEntity> entities) where TEntity : class
        {
            var queryableEntitites = entities.AsQueryable();
            var dbSet = new Mock<DbSet<TEntity>>();
            dbSet.As<IQueryable<TEntity>>().Setup(m => m.Provider).Returns(queryableEntitites.Provider);
            dbSet.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(queryableEntitites.Expression);
            dbSet.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(queryableEntitites.ElementType);
            dbSet.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(() => queryableEntitites.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<TEntity>())).Callback<TEntity>((s) => entities.Add(s));
            return dbSet;
        }

    }
}

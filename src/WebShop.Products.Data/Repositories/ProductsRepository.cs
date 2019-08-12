using System;
using System.Collections.Generic;
using System.Text;
using WebShop.Common.Database;
using WebShop.Products.Data.Entites;

namespace WebShop.Products.Data.Repositories
{
    public class ProductsRepository : GenericRepository<Product>, IProductsRepository
    {
        public ProductsRepository(ProductsDbContext dbContext) : base(dbContext)
        {

        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using WebShop.Common.Database;
using WebShop.Products.Data.Entites;

namespace WebShop.Products.Data.Repositories
{
    public interface IProductsRepository:IGenericRepository<Product>
    {
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WebShop.Products.Data
{
    public class ProductsDbContextFactory : IDesignTimeDbContextFactory<ProductsDbContext>
    {
        public ProductsDbContext CreateDbContext(string[] args)
        {
            var dbContext = new ProductsDbContext(new DbContextOptionsBuilder<ProductsDbContext>().UseSqlServer(
                new ConfigurationBuilder()
                    .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), $"appsettings.json"))
                    .Build()
                    .GetConnectionString("WebShop.Products")
                ).Options);

            dbContext.Database.Migrate();
            return dbContext;
        }
    }
}

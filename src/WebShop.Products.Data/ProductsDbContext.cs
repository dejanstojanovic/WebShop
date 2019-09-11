using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using WebShop.Products.Data.Entites;

namespace WebShop.Products.Data
{
    public class ProductsDbContext: DbContext
    {
        private readonly ILoggerFactory loggerFactory;

        public ProductsDbContext():base()
        {

        }

        public ProductsDbContext(DbContextOptions<ProductsDbContext> options)
         : base(options)
        {
        }

        public ProductsDbContext(DbContextOptions<ProductsDbContext> options, ILoggerFactory loggerFactory)
          : base(options)
        {
            this.loggerFactory = loggerFactory;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(loggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Concurrency check field
            builder.Entity<Product>()
            .Property(p => p.RowVersion)
            .IsRowVersion();

            //Apply configuration
            builder.ApplyConfiguration(new ProductConfiguration());

            base.OnModelCreating(builder);
            
        }

        public DbSet<Product> Products { get; set; }
    }
}

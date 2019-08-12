using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WebShop.Common.Database;

namespace WebShop.Products.Data.Entites
{
    public class Product:GenericEntity
    {
        [Required]
        [MaxLength(100)]
        [MinLength(3)]
        public String Name { get; set; }

        [MaxLength(500)]
        public String Description { get; set; }
    }


    public class ProductConfiguration : IEntityTypeConfiguration<Product> 
    {
        public virtual void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Id).HasDefaultValueSql("NEWID()");
        }
    }

}

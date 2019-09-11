using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using WebShop.Common.Extensions;
using WebShop.Products.Data.Entites;
using WebShop.Products.Data.Repositories;
using WebShop.Products.Data;
using Moq;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace WebShop.Products.Tests.Data
{
    public class ProductRepositoryTests
    {
        [Fact]
        public void GetByIdAsync_Returns_Product()
        {
            //Setup DbContext and DbSet mock
            var dbContextMock = new Mock<ProductsDbContext>();
            var dbSetMock = new Mock<DbSet<Product>>();
            dbSetMock.Setup(s => s.FindAsync(It.IsAny<Guid>())).Returns(Task.FromResult(new Product()));
            dbContextMock.Setup(s => s.Set<Product>()).Returns(dbSetMock.Object);

            //Execute method of SUT (ProductsRepository)
            var productRepository = new ProductsRepository(dbContextMock.Object);
            var product = productRepository.GetByIdAsync(Guid.NewGuid()).Result;

            //Assert
            Assert.NotNull(product);
            Assert.IsAssignableFrom<Product>(product);
        }

        [Fact]
        public async Task GetByIdAsync_Throws_NotFoundException()
        {
            var dbContextMock = new Mock<ProductsDbContext>();

            var dbSetMock = new Mock<DbSet<Product>>();
            dbSetMock.Setup(s => s.FindAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Product>(null));

            dbContextMock.Setup(s => s.Set<Product>()).Returns(dbSetMock.Object);
            var productRepository = new ProductsRepository(dbContextMock.Object);

            var product = await productRepository.GetByIdAsync(Guid.NewGuid());
            Assert.Null(product);

        }


    }
}

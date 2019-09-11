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
using WebShop.Common.Exceptions;
using System.Linq;

namespace WebShop.Products.Tests.Data
{
    public class ProductRepositoryTests
    {
        [Fact]
        public void GetByIdAsync_Returns_Product()
        {
            var productId = Guid.Parse("003d6613-47b1-4f0c-93e0-2a502908088f");
            var data = GetTestProducts();
            var dbContextMock = new Mock<ProductsDbContext>();

            var dbSetMock = data.GetDbSetMock<Product>();
            dbSetMock.Setup(s => s.FindAsync(It.IsAny<Guid>())).Returns(Task.FromResult(data.FirstOrDefault(p => p.Id == productId)));

            dbContextMock.Setup(s => s.Set<Product>()).Returns(dbSetMock.Object);
            var productRepository = new ProductsRepository(dbContextMock.Object);

            var product = productRepository.GetByIdAsync(productId).Result;
            Assert.NotNull(product);
            Assert.IsAssignableFrom<Product>(product);

        }

        [Fact]
        public async Task GetByIdAsync_Throws_NotFoundException()
        {
            var productId = Guid.Parse("97e389e0-8195-4e17-894e-70c01b8837c0");
            var data = GetTestProducts();
            var dbContextMock = new Mock<ProductsDbContext>();

            var dbSetMock = data.GetDbSetMock<Product>();
            dbSetMock.Setup(s => s.FindAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Product>(null));

            dbContextMock.Setup(s => s.Set<Product>()).Returns(data.GetDbSetMock<Product>().Object);
            var productRepository = new ProductsRepository(dbContextMock.Object);

            var product = await productRepository.GetByIdAsync(productId);
            Assert.Null(product);

        }

        IList<Product> GetTestProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Id= Guid.Parse("003d6613-47b1-4f0c-93e0-2a502908088f"),
                    Description="Test product 1 description",
                    Name="Product 1"
                },
                new Product()
                {
                    Id= Guid.Parse("94c76f2f-cc5e-4f45-9085-b57a2e825b27"),
                    Description="Test product 2 description",
                    Name="Product 2"
                }
            };
        }

    }
}

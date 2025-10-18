using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using technical_tests_backend_ssr.Domain;
using technical_tests_backend_ssr.Models.Entities;
using technical_tests_backend_ssr.Repositories;
using System.Linq;

namespace technical_test_backend_srr.UnitTest
{
    public class RepositoryUnitTest
    {
        private TechnicalTestDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<TechnicalTestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
                .Options;

            return new TechnicalTestDbContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldAddProduct()
        {
            var context = GetDbContext();
            var repo = new ProductRepository(context);

            var product = new Product { Name = "Test", Description = "Desc", Price = 10, Stock = 5 };
            await repo.AddAsync(product);
            await repo.SaveChangesAsync();

            var added = await context.Products.FirstOrDefaultAsync();
            Assert.NotNull(added);
            Assert.Equal("Test", added.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProduct()
        {
            var context = GetDbContext();
            context.Products.Add(new Product { Name = "Test", Description = "Desc", Price = 10, Stock = 5 });
            await context.SaveChangesAsync();

            var repo = new ProductRepository(context);
            var product = await repo.GetByIdAsync(1);

            Assert.NotNull(product);
            Assert.Equal("Test", product.Name);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            var context = GetDbContext();
            context.Products.AddRange(
                new Product { Name = "P1", Description = "D1", Price = 5, Stock = 2 },
                new Product { Name = "P2", Description = "D2", Price = 10, Stock = 3 }
            );
            await context.SaveChangesAsync();

            var repo = new ProductRepository(context);
            var products = await repo.GetAllAsync();

            Assert.Equal(2, products.Count());
        }

        [Fact]
        public async Task Update_ShouldModifyProduct()
        {
            var context = GetDbContext();
            var product = new Product { Name = "Old", Description = "Desc", Price = 5, Stock = 1 };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var repo = new ProductRepository(context);
            product.Name = "New";
            repo.Update(product);
            await repo.SaveChangesAsync();

            var updated = await context.Products.FindAsync(product.Id);
            Assert.Equal("New", updated.Name);
        }

        [Fact]
        public async Task Delete_ShouldRemoveProduct()
        {
            var context = GetDbContext();
            var product = new Product { Name = "DeleteMe", Description = "Desc", Price = 5, Stock = 1 };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var repo = new ProductRepository(context);
            repo.Delete(product);
            await repo.SaveChangesAsync();

            var deleted = await context.Products.FindAsync(product.Id);
            Assert.Null(deleted);
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using technical_tests_backend_ssr.Models.DTOs;
using technical_tests_backend_ssr.Models.Entities;
using technical_tests_backend_ssr.Repositories;
using technical_tests_backend_ssr.Services;
using Xunit;

namespace technical_test_backend_srr.UnitTest
{
    public class ServiceUnitTest
    {
        private readonly Mock<IProductRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ProductService _service;

        public ServiceUnitTest()
        {
            _repoMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new ProductService(_repoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_Returns_Mapped_Products()
        {
            var products = new List<Product> { new Product { Id = 1, Name = "P1", Price = 10 } };
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);
            _mapperMock.Setup(m => m.Map<IEnumerable<ProductDto>>(It.IsAny<IEnumerable<Product>>()))
                       .Returns(new List<ProductDto> { new ProductDto { Id = 1, Name = "P1", Price = 10 } });

            var result = await _service.GetAllAsync();

            Assert.Single(result);
            Assert.Equal("P1", ((List<ProductDto>)result)[0].Name);
        }

        [Fact]
        public async Task GetByIdAsync_Returns_Mapped_Product()
        {
            var product = new Product { Id = 1, Name = "P1", Price = 10 };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            _mapperMock.Setup(m => m.Map<ProductDto>(product)).Returns(new ProductDto { Id = 1, Name = "P1", Price = 10 });

            var result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("P1", result!.Name);
        }

        [Fact]
        public async Task CreateAsync_Adds_And_Returns_ProductDto()
        {
            var dto = new ProductDto { Name = "New", Price = 5 };
            var product = new Product { Id = 1, Name = "New", Price = 5 };

            _mapperMock.Setup(m => m.Map<Product>(dto)).Returns(product);
            _mapperMock.Setup(m => m.Map<ProductDto>(product)).Returns(new ProductDto { Id = 1, Name = "New", Price = 5 });

            var result = await _service.CreateAsync(dto);

            _repoMock.Verify(r => r.AddAsync(product), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
            Assert.Equal(1, result.Id);
            Assert.Equal("New", result.Name);
        }

        [Fact]
        public async Task UpdateAsync_Updates_Product_When_Exists()
        {
            var dto = new ProductDto { Name = "Updated", Price = 20 };
            var product = new Product { Id = 1, Name = "Old", Price = 10 };

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            _mapperMock.Setup(m => m.Map(dto, product));

            _mapperMock.Setup(m => m.Map<ProductDto>(product)).Returns(new ProductDto { Id = 1, Name = "Updated", Price = 20 });

            var result = await _service.UpdateAsync(1, dto);

            _repoMock.Verify(r => r.Update(product), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
            Assert.Equal("Updated", result!.Name);
        }

        [Fact]
        public async Task UpdateAsync_Returns_Null_When_Product_DoesNotExist()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Product?)null);

            var result = await _service.UpdateAsync(1, new ProductDto());

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_Returns_True_When_Product_Deleted()
        {
            var product = new Product { Id = 1 };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            var result = await _service.DeleteAsync(1);

            _repoMock.Verify(r => r.Delete(product), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_Returns_False_When_Product_NotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Product?)null);

            var result = await _service.DeleteAsync(1);

            Assert.False(result);
        }
    }
}

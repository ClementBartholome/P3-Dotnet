using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using P3AddNewFunctionalityDotNetCore.Data;
using Microsoft.Extensions.Configuration;
using P3AddNewFunctionalityDotNetCore.Models;
using Xunit.Abstractions;


namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class DbContextFixture
    {
        public ProductRepository ProductRepository { get; }

        public DbContextFixture()
        {
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseInMemoryDatabase(databaseName: $"P3AddNewFunctionalityDb{Guid.NewGuid()}")
                .Options;

            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string>("ConnectionStrings:P3Referential",
                    @"Server=localhost\\MSSQLSERVER01;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true")
            });
            var config = configBuilder.Build();

            var context = new P3Referential(options, config);
            ProductRepository = new ProductRepository(context);
        }
    }

    public class ProductServiceIntegrationTests : IClassFixture<DbContextFixture>
    {
        private readonly ITestOutputHelper _output;
        private readonly ProductService _productService;

        private readonly Cart _cart;
        private readonly ProductViewModel _initialProduct;

        public ProductServiceIntegrationTests(ITestOutputHelper output, DbContextFixture fixture)
        {
            _output = output;
            _cart = new Cart();
            _productService = new ProductService(_cart, fixture.ProductRepository, null, null);


            _initialProduct = new ProductViewModel
            {
                Name = "Test Product",
                Stock = "10",
                Price = "100.99",
                Description = "Test Description",
                Details = "Test Details"
            };
        }

        [Fact]
        public async void CheckProductDeletion()
        {
            // Arrange
            await _productService.SaveProduct(_initialProduct);
            var savedProduct = _productService.GetAllProductsViewModel().First();

            // Act
            _productService.DeleteProduct(savedProduct.Id);

            // Assert
            var deletedProduct = _productService.GetProductByIdViewModel(savedProduct.Id);
            Assert.Null(deletedProduct);
        }

        [Fact]
        public async void CheckProductCreation()
        {
            // Arrange
            await _productService.SaveProduct(_initialProduct);

            // Act
            var savedProduct = _productService.GetAllProductsViewModel().First();

            // Assert
            Assert.NotNull(savedProduct);
            Assert.Equal(_initialProduct.Name, savedProduct.Name);
            Assert.Equal(_initialProduct.Stock, savedProduct.Stock);
            Assert.Equal(_initialProduct.Price, savedProduct.Price);
            Assert.Equal(_initialProduct.Description, savedProduct.Description);
            Assert.Equal(_initialProduct.Details, savedProduct.Details);
        }

        [Fact]
        public async Task CheckUpdateProductQuantities()
        {
            // Arrange
            _productService.SaveProduct(_initialProduct);
            var product = _productService.GetAllProducts().First();
            _cart.AddItem(product, 5);
            _output.WriteLine("Added product to cart. Stock before update: {0}", product.Quantity);

            // Act
            _productService.UpdateProductQuantities();

            _output.WriteLine("Updated product quantities. Stock after update: {0}", product.Quantity);

            // Assert
            var updatedProduct = _productService.GetProductByIdViewModel(product.Id);
            Assert.Equal(5, int.Parse(updatedProduct.Stock));
        }
    }
}
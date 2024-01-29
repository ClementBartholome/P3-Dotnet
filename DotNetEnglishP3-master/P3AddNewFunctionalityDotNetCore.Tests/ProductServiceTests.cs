using System.Collections.Generic;
using Xunit;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IStringLocalizer<ProductService>> _localizerMock;
        private readonly ProductService _productService;
        private readonly Mock<IProductRepository> _mockProductRepository;

        public ProductServiceTests()
        {
            _localizerMock = new Mock<IStringLocalizer<ProductService>>();
            _mockProductRepository = new Mock<IProductRepository>();
            _productService = new ProductService(null, _mockProductRepository.Object, null, _localizerMock.Object);
        }

        [Fact]
        public void CheckProductModelErrors_ReturnsMissingNameError_WhenNameIsNull()
        {
            // Arrange
            var product = new ProductViewModel { Name = null, Stock = 10, Price = 1.99 };
            var expectedError = "MissingName";
            var expectedErrors = new List<string> { expectedError };
            _localizerMock.Setup(_ => _[expectedError]).Returns(new LocalizedString(expectedError, expectedError));

            // Act
            var result = _productService.CheckProductModelErrors(product);

            // Assert
            Assert.Equal(expectedErrors, result);
        }

        [Fact]
        public void CheckProductModelErrors_ReturnsStockNotGreaterThanZeroError_WhenStockIsZero()
        {
            // Arrange
            var product = new ProductViewModel { Name = "Test", Stock = 0, Price = 1.99 };
            var expectedError = "StockNotGreaterThanZero";
            var expectedErrors = new List<string> { expectedError };
            _localizerMock.Setup(_ => _[expectedError]).Returns(new LocalizedString(expectedError, expectedError));

            // Act
            var result = _productService.CheckProductModelErrors(product);

            // Assert
            Assert.Equal(expectedErrors, result);
        }

        [Fact]
        public void CheckProductModelErrors_ReturnsPriceNotGreaterThanZeroError_WhenPriceIsZero()
        {
            // Arrange
            var product = new ProductViewModel { Name = "Test", Stock = 10, Price = 0 };
            var expectedError = "PriceNotGreaterThanZero";
            var expectedErrors = new List<string> { expectedError };
            _localizerMock.Setup(_ => _[expectedError]).Returns(new LocalizedString(expectedError, expectedError));

            // Act
            var result = _productService.CheckProductModelErrors(product);

            // Assert
            Assert.Equal(expectedErrors, result);
        }
        
        [Fact]
        public void GetAllProducts_ReturnsAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Name = "Test1", Quantity = 10, Price = 1.99 },
                new Product { Name = "Test2", Quantity = 10, Price = 1.99 },
                new Product { Name = "Test3", Quantity = 10, Price = 1.99 }
            };
            _mockProductRepository.Setup(repo => repo.GetAllProducts()).Returns(products);

            // Act
            var result = _productService.GetAllProductsViewModel();

            // Assert
            Assert.Equal(products.Count, result.Count);
        }
    }
}
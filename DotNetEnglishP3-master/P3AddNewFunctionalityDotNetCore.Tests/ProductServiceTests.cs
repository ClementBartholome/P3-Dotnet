using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Resources;
using Xunit;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using ProductService = P3AddNewFunctionalityDotNetCore.Models.Services.ProductService;
using ServiceResources = P3AddNewFunctionalityDotNetCore.Resources.Models.Services;

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
        public void ProductViewModel_ReturnsMissingNameError_WhenNameIsNull()
        {
            // Arrange
            var product = new ProductViewModel { Name = null, Stock = "10", Price = "1.99" };
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(product, new ValidationContext(product), validationResults, true);
            var resourceManager = new ResourceManager(typeof(ServiceResources.ProductService));
            var expectedErrorMessage = resourceManager.GetString("MissingName");

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, vr => vr.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public void ProductViewModel_ReturnsStockNotGreaterThanZeroError_WhenStockIsZero()
        {
            // Arrange
            var product = new ProductViewModel { Name = "Test", Stock = "0", Price = "1.99" };
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(product, new ValidationContext(product), validationResults, true);
            var resourceManager = new ResourceManager(typeof(ServiceResources.ProductService));
            var expectedErrorMessage = resourceManager.GetString("StockNotGreaterThanZero");
            
            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, vr => vr.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public void ProductViewModel_ReturnsPriceNotGreaterThanZeroError_WhenPriceIsZero()
        {
            // Arrange
            var product = new ProductViewModel { Name = "Test", Stock = "10", Price = "0" };
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(product, new ValidationContext(product), validationResults, true);
            var resourceManager = new ResourceManager(typeof(ServiceResources.ProductService));
            var expectedErrorMessage = resourceManager.GetString("PriceNotGreaterThanZero");
            
            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, vr => vr.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public void ProductViewModel_ReturnsMissingPriceError_WhenPriceIsNull()
        {
            // Arrange
            var product = new ProductViewModel { Name = "Test", Stock = "10", Price = null };
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(product, new ValidationContext(product), validationResults, true);
            var resourceManager = new ResourceManager(typeof(ServiceResources.ProductService));
            var expectedErrorMessage = resourceManager.GetString("MissingPrice");
            
            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, vr => vr.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public void ProductViewModel_ReturnsPriceNotANumberError_WhenPriceIsNotANumber()
        {
            // Arrange
            var product = new ProductViewModel { Name = "Test", Stock = "10", Price = "NotANumber" };
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(product, new ValidationContext(product), validationResults, true);
            var resourceManager = new ResourceManager(typeof(ServiceResources.ProductService));
            var expectedErrorMessage = resourceManager.GetString("PriceNotANumber");

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, vr => vr.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public void ProductViewModel_ReturnsMissingStockError_WhenStockIsNull()
        {
            // Arrange
            var product = new ProductViewModel { Name = "Test", Stock = null, Price = "1.99" };
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(product, new ValidationContext(product), validationResults, true);
            var resourceManager = new ResourceManager(typeof(ServiceResources.ProductService));
            var expectedErrorMessage = resourceManager.GetString("MissingStock");
            
            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, vr => vr.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public void ProductViewModel_ReturnsStockNotAnIntegerError_WhenStockIsNotAnInteger()
        {
            // Arrange
            var product = new ProductViewModel { Name = "Test", Stock = "1.99", Price = "1.99" };
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(product, new ValidationContext(product), validationResults, true);
            var resourceManager = new ResourceManager(typeof(ServiceResources.ProductService));
            var expectedErrorMessage = resourceManager.GetString("StockNotAnInteger");

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, vr => vr.ErrorMessage == expectedErrorMessage);
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
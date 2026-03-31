using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tebenkova_Models.Interfaces;
using tebenkova_Models.Models;
using tebenkova_Services.Services;

namespace tebenkova_Tests.Services
{
    [TestClass]
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _mockProductRepository;
        private Mock<ISupplierRepository> _mockSupplierRepository;
        private ProductService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _mockSupplierRepository = new Mock<ISupplierRepository>();
            _service = new ProductService(_mockProductRepository.Object, _mockSupplierRepository.Object);
        }

        [TestMethod]
        public async Task GetAllProductsAsync_ShouldReturnAllProducts()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Розы", Article = "ROS001" },
                new Product { Id = 2, Name = "Тюльпаны", Article = "TUL001" }
            };
            _mockProductRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

            var result = await _service.GetAllProductsAsync();

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Розы", result.First().Name);
        }

        [TestMethod]
        public async Task GetProductByIdAsync_WithValidId_ShouldReturnProduct()
        {
            var product = new Product
            {
                Id = 1,
                Name = "Розы",
                Article = "ROS001",
                ProductType = new ProductType { Name = "Цветы" },
                Supplier = new Supplier { Name = "Поставщик 1" }
            };
            _mockProductRepository.Setup(r => r.GetByIdWithDetailsAsync(1)).ReturnsAsync(product);

            var result = await _service.GetProductByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Розы", result.Name);
        }

        [TestMethod]
        public async Task CreateProductAsync_WithValidData_ShouldCreateProduct()
        {
            var product = new Product
            {
                Article = "NEW001",
                Name = "Новый товар",
                QuantityInStock = 100
            };
            var createdProduct = new Product
            {
                Id = 1,
                Article = "NEW001",
                Name = "Новый товар",
                QuantityInStock = 100
            };
            _mockProductRepository.Setup(r => r.AddAsync(It.IsAny<Product>())).ReturnsAsync(createdProduct);

            var result = await _service.CreateProductAsync(product);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("NEW001", result.Article);
        }

        [TestMethod]
        public async Task GetLowStockProductsAsync_ShouldReturnProductsBelowMinStock()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Товар 1", QuantityInStock = 5, MinStockQuantity = 10 },
                new Product { Id = 2, Name = "Товар 2", QuantityInStock = 20, MinStockQuantity = 10 },
                new Product { Id = 3, Name = "Товар 3", QuantityInStock = 0, MinStockQuantity = 5 }
            };
            _mockProductRepository.Setup(r => r.GetLowStockAsync()).ReturnsAsync(products.Where(p => p.QuantityInStock <= p.MinStockQuantity));

            var result = await _service.GetLowStockProductsAsync();

            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public async Task GetStockSummaryAsync_ShouldReturnCorrectSummary()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Товар 1",
                    QuantityInStock = 50,
                    MinStockQuantity = 10,
                    CostPrice = 100,
                    ProductType = new ProductType { Name = "Тип 1" }
                },
                new Product
                {
                    Id = 2,
                    Name = "Товар 2",
                    QuantityInStock = 5,
                    MinStockQuantity = 20,
                    CostPrice = 200,
                    ProductType = new ProductType { Name = "Тип 1" }
                },
                new Product
                {
                    Id = 3,
                    Name = "Товар 3",
                    QuantityInStock = 0,
                    MinStockQuantity = 10,
                    CostPrice = 300,
                    ProductType = new ProductType { Name = "Тип 2" }
                }
            };
            _mockProductRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

            var result = await _service.GetStockSummaryAsync();

            Assert.AreEqual(3, result.TotalProducts);
            Assert.AreEqual(55, result.TotalItemsInStock);
            Assert.AreEqual(2, result.LowStockItems);
            Assert.AreEqual(1, result.OutOfStockItems);
            Assert.AreEqual(50 * 100 + 5 * 200 + 0 * 300, result.TotalStockValue);
            Assert.AreEqual(2, result.ProductsByType["Тип 1"]);
            Assert.AreEqual(1, result.ProductsByType["Тип 2"]);
        }
    }
}

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
    public class SupplierServiceTests
    {
        private Mock<ISupplierRepository> _mockSupplierRepository;
        private Mock<IProductRepository> _mockProductRepository;
        private SupplierService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockSupplierRepository = new Mock<ISupplierRepository>();
            _mockProductRepository = new Mock<IProductRepository>();
            _service = new SupplierService(_mockSupplierRepository.Object, _mockProductRepository.Object);
        }

        [TestMethod]
        public async Task GetAllSuppliersAsync_ShouldReturnAllSuppliers()
        {
            var suppliers = new List<Supplier>
            {
                new Supplier { Id = 1, Name = "Поставщик 1" },
                new Supplier { Id = 2, Name = "Поставщик 2" }
            };
            _mockSupplierRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(suppliers);

            var result = await _service.GetAllSuppliersAsync();

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Поставщик 1", result.First().Name);
        }

        [TestMethod]
        public async Task GetSupplierByIdAsync_WithValidId_ShouldReturnSupplier()
        {
            var supplier = new Supplier
            {
                Id = 1,
                Name = "Поставщик 1",
                Products = new List<Product>
                {
                    new Product { Id = 1, Name = "Товар 1" }
                }
            };
            _mockSupplierRepository.Setup(r => r.GetByIdWithProductsAsync(1)).ReturnsAsync(supplier);

            var result = await _service.GetSupplierByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual(1, result.Products.Count);
        }

        [TestMethod]
        public async Task CreateSupplierAsync_WithValidData_ShouldCreateSupplier()
        {
            var supplier = new Supplier
            {
                Name = "Новый поставщик",
                INN = "1234567890",
                Phone = "+7 (342) 123-45-67",
                Email = "supplier@test.com"
            };
            var createdSupplier = new Supplier
            {
                Id = 1,
                Name = "Новый поставщик",
                Rating = 0
            };
            _mockSupplierRepository.Setup(r => r.AddAsync(It.IsAny<Supplier>())).ReturnsAsync(createdSupplier);

            var result = await _service.CreateSupplierAsync(supplier);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Новый поставщик", result.Name);
        }

        [TestMethod]
        public async Task DeleteSupplierAsync_WithNoProducts_ShouldDeleteSupplier()
        {
            _mockProductRepository.Setup(r => r.GetBySupplierIdAsync(1)).ReturnsAsync(new List<Product>());
            _mockSupplierRepository.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            await _service.DeleteSupplierAsync(1);

            _mockSupplierRepository.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [TestMethod]
        public async Task GetSupplierStatisticsAsync_ShouldReturnCorrectStatistics()
        {
            var supplier = new Supplier
            {
                Id = 1,
                Name = "Поставщик 1",
                Rating = 5,
                Products = new List<Product>
                {
                    new Product
                    {
                        Name = "Товар 1",
                        QuantityInStock = 50,
                        CostPrice = 100,
                        ProductType = new ProductType { Name = "Тип 1" }
                    },
                    new Product
                    {
                        Name = "Товар 2",
                        QuantityInStock = 30,
                        CostPrice = 200,
                        ProductType = new ProductType { Name = "Тип 1" }
                    },
                    new Product
                    {
                        Name = "Товар 3",
                        QuantityInStock = 20,
                        CostPrice = 300,
                        ProductType = new ProductType { Name = "Тип 2" }
                    }
                }
            };
            _mockSupplierRepository.Setup(r => r.GetByIdWithProductsAsync(1)).ReturnsAsync(supplier);

            var result = await _service.GetSupplierStatisticsAsync(1);

            Assert.AreEqual(1, result.SupplierId);
            Assert.AreEqual("Поставщик 1", result.SupplierName);
            Assert.AreEqual(3, result.TotalProducts);
            Assert.AreEqual(50 * 100 + 30 * 200 + 20 * 300, result.TotalStockValue);
            Assert.AreEqual((100 + 200 + 300) / 3m, result.AverageProductPrice);
            Assert.AreEqual(2, result.ProductsByType["Тип 1"]);
            Assert.AreEqual(1, result.ProductsByType["Тип 2"]);
            Assert.AreEqual(5, result.Rating);
        }
    }
}

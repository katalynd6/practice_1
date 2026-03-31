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
    public class ReportServiceTests
    {
        private Mock<IPartnerRepository> _mockPartnerRepository;
        private Mock<IProductRepository> _mockProductRepository;
        private Mock<ISaleHistoryRepository> _mockSaleHistoryRepository;
        private Mock<ISupplierRepository> _mockSupplierRepository;
        private ReportService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockPartnerRepository = new Mock<IPartnerRepository>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockSaleHistoryRepository = new Mock<ISaleHistoryRepository>();
            _mockSupplierRepository = new Mock<ISupplierRepository>();
            _service = new ReportService(
                _mockPartnerRepository.Object,
                _mockProductRepository.Object,
                _mockSaleHistoryRepository.Object,
                _mockSupplierRepository.Object);
        }

        [TestMethod]
        public async Task GetDashboardSummaryAsync_ShouldReturnCorrectSummary()
        {
            var partners = new List<Partner>
            {
                new Partner
                {
                    Id = 1,
                    Name = "Партнер 1",
                    SalesHistories = new List<SaleHistory>
                    {
                        new SaleHistory { Quantity = 10, SalePrice = 1000 }
                    }
                },
                new Partner
                {
                    Id = 2,
                    Name = "Партнер 2",
                    SalesHistories = new List<SaleHistory>()
                }
            };

            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Товар 1", QuantityInStock = 50, MinStockQuantity = 10 },
                new Product { Id = 2, Name = "Товар 2", QuantityInStock = 5, MinStockQuantity = 20 },
                new Product { Id = 3, Name = "Товар 3", QuantityInStock = 0, MinStockQuantity = 10 }
            };

            var sales = new List<SaleHistory>
            {
                new SaleHistory { Id = 1, Quantity = 10, SalePrice = 1000 },
                new SaleHistory { Id = 2, Quantity = 5, SalePrice = 2000 },
                new SaleHistory { Id = 3, Quantity = 2, SalePrice = 3000 }
            };

            var suppliers = new List<Supplier>
            {
                new Supplier { Id = 1, Name = "Поставщик 1" },
                new Supplier { Id = 2, Name = "Поставщик 2" }
            };

            _mockPartnerRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(partners);
            _mockProductRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(products);
            _mockSaleHistoryRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(sales);
            _mockSupplierRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(suppliers);

            var result = await _service.GetDashboardSummaryAsync();

            Assert.AreEqual(2, result.TotalPartners);
            Assert.AreEqual(3, result.TotalProducts);
            Assert.AreEqual(2, result.TotalSuppliers);
            Assert.AreEqual(10 * 1000 + 5 * 2000 + 2 * 3000, result.TotalSales);
            Assert.AreEqual(3, result.TotalTransactions);
            Assert.AreEqual((10 * 1000 + 5 * 2000 + 2 * 3000) / 3m, result.AverageTransactionValue);
            Assert.AreEqual(2, result.LowStockProducts); // Товар 2 и Товар 3
        }

        [TestMethod]
        public async Task GetSalesReportAsync_ShouldReturnCorrectReport()
        {
            var startDate = new DateTime(2026, 3, 1);
            var endDate = new DateTime(2026, 3, 31);

            var sales = new List<SaleHistory>
            {
                new SaleHistory
                {
                    Id = 1,
                    SaleDate = new DateTime(2026, 3, 5),
                    Quantity = 10,
                    SalePrice = 1000,
                    Product = new Product { Name = "Товар 1" }
                },
                new SaleHistory
                {
                    Id = 2,
                    SaleDate = new DateTime(2026, 3, 5),
                    Quantity = 5,
                    SalePrice = 2000,
                    Product = new Product { Name = "Товар 2" }
                },
                new SaleHistory
                {
                    Id = 3,
                    SaleDate = new DateTime(2026, 3, 10),
                    Quantity = 2,
                    SalePrice = 3000,
                    Product = new Product { Name = "Товар 1" }
                }
            };

            _mockSaleHistoryRepository.Setup(r => r.GetByDateRangeAsync(startDate, endDate)).ReturnsAsync(sales);

            var result = await _service.GetSalesReportAsync(startDate, endDate);

            Assert.AreEqual(startDate, result.StartDate);
            Assert.AreEqual(endDate, result.EndDate);
            Assert.AreEqual(10 * 1000 + 5 * 2000 + 2 * 3000, result.TotalSales);
            Assert.AreEqual(3, result.TotalTransactions);

            // Проверка топ продуктов
            Assert.AreEqual(2, result.TopProducts.Count);
            Assert.AreEqual("Товар 1", result.TopProducts[0].ProductName);
            Assert.AreEqual(12, result.TopProducts[0].Quantity); // 10 + 2
            Assert.AreEqual(10 * 1000 + 2 * 3000, result.TopProducts[0].TotalAmount);

            Assert.AreEqual("Товар 2", result.TopProducts[1].ProductName);
            Assert.AreEqual(5, result.TopProducts[1].Quantity);
            Assert.AreEqual(5 * 2000, result.TopProducts[1].TotalAmount);
        }
    }
}

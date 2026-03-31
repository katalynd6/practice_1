using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tebenkova_Models.Interfaces;
using tebenkova_Models.Models;
using tebenkova_Services.Services;

namespace tebenkova_Tests.Services
{
    [TestClass]
    public class PartnerServiceTests
    {
        private Mock<IPartnerRepository> _mockPartnerRepository;
        private Mock<ISaleHistoryRepository> _mockSaleHistoryRepository;
        private PartnerService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockPartnerRepository = new Mock<IPartnerRepository>();
            _mockSaleHistoryRepository = new Mock<ISaleHistoryRepository>();
            _service = new PartnerService(_mockPartnerRepository.Object, _mockSaleHistoryRepository.Object);
        }

        [TestMethod]
        public async Task GetAllPartnersAsync_ShouldReturnAllPartners()
        {
            var partners = new List<Partner>
            {
                new Partner { Id = 1, Name = "Партнер 1" },
                new Partner { Id = 2, Name = "Партнер 2" }
            };
            _mockPartnerRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(partners);

            var result = await _service.GetAllPartnersAsync();

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Партнер 1", result.First().Name);
        }

        [TestMethod]
        public async Task GetPartnerByIdAsync_WithValidId_ShouldReturnPartner()
        {
            var partner = new Partner { Id = 1, Name = "Партнер 1" };
            _mockPartnerRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(partner);

            var result = await _service.GetPartnerByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Партнер 1", result.Name);
        }

        [TestMethod]
        public async Task GetPartnerByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            _mockPartnerRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Partner)null);

            var result = await _service.GetPartnerByIdAsync(999);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetPartnerDetailAsync_ShouldReturnPartnerWithDetails()
        {
            var partner = new Partner
            {
                Id = 1,
                Name = "Партнер 1",
                SalesHistories = new List<SaleHistory>
                {
                    new SaleHistory { Id = 1, Quantity = 10, SalePrice = 100 }
                }
            };
            _mockPartnerRepository.Setup(r => r.GetByIdWithDetailsAsync(1)).ReturnsAsync(partner);

            var result = await _service.GetPartnerDetailAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.SalesHistories.Count);
        }

        [TestMethod]
        public async Task CreatePartnerAsync_WithValidData_ShouldCreatePartner()
        {
            var partner = new Partner
            {
                Name = "Новый партнер",
                Rating = 5,
                INN = "1234567890",
                Phone = "+7 (342) 123-45-67",
                Email = "test@test.com"
            };
            var createdPartner = new Partner
            {
                Id = 1,
                Name = "Новый партнер",
                Rating = 5
            };
            _mockPartnerRepository.Setup(r => r.AddAsync(It.IsAny<Partner>())).ReturnsAsync(createdPartner);

            var result = await _service.CreatePartnerAsync(partner);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Новый партнер", result.Name);
        }


        [TestMethod]
        public async Task UpdatePartnerAsync_WithValidData_ShouldUpdatePartner()
        {
            var partner = new Partner { Id = 1, Name = "Обновленный партнер" };
            _mockPartnerRepository.Setup(r => r.ExistsAsync(1)).ReturnsAsync(true);
            _mockPartnerRepository.Setup(r => r.UpdateAsync(It.IsAny<Partner>())).Returns(Task.CompletedTask);

            await _service.UpdatePartnerAsync(partner);

            _mockPartnerRepository.Verify(r => r.UpdateAsync(partner), Times.Once);
        }


        [TestMethod]
        public async Task DeletePartnerAsync_ShouldDeletePartner()
        {
            _mockPartnerRepository.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            await _service.DeletePartnerAsync(1);

            _mockPartnerRepository.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [TestMethod]
        public async Task CalculateDiscountAsync_WithNoSales_ShouldReturnZero()
        {
            _mockSaleHistoryRepository.Setup(r => r.GetByPartnerIdAsync(1))
                .ReturnsAsync(new List<SaleHistory>());

            var result = await _service.CalculateDiscountAsync(1);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task CalculateDiscountAsync_WithSalesLess10000_ShouldReturnZero()
        {
            var sales = new List<SaleHistory>
            {
                new SaleHistory { Quantity = 1, SalePrice = 5000 }
            };
            _mockSaleHistoryRepository.Setup(r => r.GetByPartnerIdAsync(1)).ReturnsAsync(sales);

            var result = await _service.CalculateDiscountAsync(1);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task CalculateDiscountAsync_WithSalesBetween10000And50000_ShouldReturn5()
        {
            var sales = new List<SaleHistory>
            {
                new SaleHistory { Quantity = 2, SalePrice = 10000 }
            };
            _mockSaleHistoryRepository.Setup(r => r.GetByPartnerIdAsync(1)).ReturnsAsync(sales);

            var result = await _service.CalculateDiscountAsync(1);

            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public async Task CalculateDiscountAsync_WithSalesBetween50000And300000_ShouldReturn10()
        {
            var sales = new List<SaleHistory>
            {
                new SaleHistory { Quantity = 5, SalePrice = 20000 }
            };
            _mockSaleHistoryRepository.Setup(r => r.GetByPartnerIdAsync(1)).ReturnsAsync(sales);

            var result = await _service.CalculateDiscountAsync(1);

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public async Task CalculateDiscountAsync_WithSalesMore300000_ShouldReturn15()
        {
            var sales = new List<SaleHistory>
            {
                new SaleHistory { Quantity = 10, SalePrice = 40000 }
            };
            _mockSaleHistoryRepository.Setup(r => r.GetByPartnerIdAsync(1)).ReturnsAsync(sales);

            var result = await _service.CalculateDiscountAsync(1);

            Assert.AreEqual(15, result);
        }

        [TestMethod]
        public async Task SearchPartnersAsync_WithSearchTerm_ShouldReturnMatchingPartners()
        {
            var allPartners = new List<Partner>
            {
                new Partner { Id = 1, Name = "ООО Ромашка" },
                new Partner { Id = 2, Name = "ИП Петров" }
            };
            var filteredPartners = allPartners.Where(p => p.Name.Contains("Ромашка")).ToList();

            _mockPartnerRepository.Setup(r => r.SearchAsync("Ромашка"))
                .ReturnsAsync(filteredPartners);

            var result = await _service.SearchPartnersAsync("Ромашка");

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("ООО Ромашка", result.First().Name);
        }
    }
}
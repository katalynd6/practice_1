using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tebenkova_Models.Interfaces;
using tebenkova_Models.Models;

namespace tebenkova_Tests.Repositories
{
    [TestClass]
    public class PartnerRepositoryTests
    {
        private Mock<IPartnerRepository> _mockRepository;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<IPartnerRepository>();
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnAllPartners()
        {
            var expected = new List<Partner>
            {
                new Partner { Id = 1, Name = "Партнер 1" },
                new Partner { Id = 2, Name = "Партнер 2" }
            };
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(expected);

            var result = await _mockRepository.Object.GetAllAsync();

            Assert.AreEqual(2, result.Count());
            CollectionAssert.Contains(expected.ToList(), result.First());
        }

        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnCorrectPartner()
        {
            var expected = new Partner { Id = 1, Name = "Партнер 1" };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(expected);

            var result = await _mockRepository.Object.GetByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Партнер 1", result.Name);
        }

        [TestMethod]
        public async Task AddAsync_ShouldAddPartner()
        {
            var newPartner = new Partner { Id = 1, Name = "Новый партнер" };
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Partner>())).ReturnsAsync(newPartner);

            var result = await _mockRepository.Object.AddAsync(newPartner);

            Assert.IsNotNull(result);
            Assert.AreEqual("Новый партнер", result.Name);
        }

        [TestMethod]
        public async Task ExistsAsync_WithExistingId_ShouldReturnTrue()
        {
            _mockRepository.Setup(r => r.ExistsAsync(1)).ReturnsAsync(true);

            var result = await _mockRepository.Object.ExistsAsync(1);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task ExistsAsync_WithNonExistingId_ShouldReturnFalse()
        {
            _mockRepository.Setup(r => r.ExistsAsync(999)).ReturnsAsync(false);

            var result = await _mockRepository.Object.ExistsAsync(999);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task SearchAsync_ShouldReturnMatchingPartners()
        {
            var expected = new List<Partner>
            {
                new Partner { Id = 1, Name = "ООО Ромашка" }
            };
            _mockRepository.Setup(r => r.SearchAsync("Ромашка")).ReturnsAsync(expected);

            var result = await _mockRepository.Object.SearchAsync("Ромашка");

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("ООО Ромашка", result.First().Name);
        }
    }
}

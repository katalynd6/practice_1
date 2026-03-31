using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tebenkova_Models.Models;
using tebenkova_Services.Helpers;

namespace tebenkova_Tests.Helpers
{
    [TestClass]
    public class DiscountHelperTests
    {
        [TestMethod]
        public void CalculateDiscount_NoSales_ReturnsZero()
        {
            var sales = new List<SaleHistory>();

            var result = DiscountHelper.CalculateDiscount(sales);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CalculateDiscount_TotalLess10000_ReturnsZero()
        {
            var sales = new List<SaleHistory>
            {
                new SaleHistory { Quantity = 1, SalePrice = 5000 }
            };

            var result = DiscountHelper.CalculateDiscount(sales);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CalculateDiscount_TotalExactly10000_Returns5()
        {
            var sales = new List<SaleHistory>
            {
                new SaleHistory { Quantity = 1, SalePrice = 10000 }
            };

            var result = DiscountHelper.CalculateDiscount(sales);

            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void CalculateDiscount_TotalBetween10000And50000_Returns5()
        {
            var sales = new List<SaleHistory>
            {
                new SaleHistory { Quantity = 2, SalePrice = 10000 }
            };

            var result = DiscountHelper.CalculateDiscount(sales);

            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void CalculateDiscount_TotalExactly50000_Returns10()
        {
            var sales = new List<SaleHistory>
            {
                new SaleHistory { Quantity = 5, SalePrice = 10000 }
            };

            var result = DiscountHelper.CalculateDiscount(sales);

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void CalculateDiscount_TotalBetween50000And300000_Returns10()
        {
            var sales = new List<SaleHistory>
            {
                new SaleHistory { Quantity = 10, SalePrice = 10000 }
            };

            var result = DiscountHelper.CalculateDiscount(sales);

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void CalculateDiscount_TotalExactly300000_Returns15()
        {
            var sales = new List<SaleHistory>
            {
                new SaleHistory { Quantity = 30, SalePrice = 10000 }
            };

            var result = DiscountHelper.CalculateDiscount(sales);

            Assert.AreEqual(15, result);
        }

        [TestMethod]
        public void CalculateDiscount_TotalMore300000_Returns15()
        {
            var sales = new List<SaleHistory>
            {
                new SaleHistory { Quantity = 40, SalePrice = 10000 }
            };

            var result = DiscountHelper.CalculateDiscount(sales);

            Assert.AreEqual(15, result);
        }

        [TestMethod]
        public void CalculateDiscount_MultipleSales_CalculatesCorrectly()
        {
            var sales = new List<SaleHistory>
            {
                new SaleHistory { Quantity = 3, SalePrice = 20000 },
                new SaleHistory { Quantity = 2, SalePrice = 15000 },
                new SaleHistory { Quantity = 5, SalePrice = 10000 }
            }; // 60000 + 30000 + 50000 = 140000

            var result = DiscountHelper.CalculateDiscount(sales);

            Assert.AreEqual(10, result);
        }     
    }
}

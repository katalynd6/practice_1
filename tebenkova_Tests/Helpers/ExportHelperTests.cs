using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tebenkova_Services.Helpers;

namespace tebenkova_Tests.Helpers
{
    [TestClass]
    public class ExportHelperTests
    {
        private class TestItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
        }

        [TestMethod]
        public void FormatDate_ShouldFormatCorrectly()
        {
            var date = new DateTime(2026, 3, 13);
            var result = ExportHelper.FormatDate(date);
            Assert.AreEqual("13.03.2026", result);
        }

        [TestMethod]
        public void FormatDateTime_ShouldFormatCorrectly()
        {
            var dateTime = new DateTime(2026, 3, 13, 14, 30, 0);
            var result = ExportHelper.FormatDateTime(dateTime);
            Assert.AreEqual("13.03.2026 14:30", result);
        }
    }
}

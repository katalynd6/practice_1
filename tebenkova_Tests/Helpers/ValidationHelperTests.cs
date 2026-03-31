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
    public class ValidationHelperTests
    {
        [TestMethod]
        public void IsValidINN_Valid10DigitINN_ReturnsTrue()
        {
            var result = ValidationHelper.IsValidINN("1234567890");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidINN_Valid12DigitINN_ReturnsTrue()
        {
            var result = ValidationHelper.IsValidINN("123456789012");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidINN_InvalidLength_ReturnsFalse()
        {
            var result = ValidationHelper.IsValidINN("12345");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidINN_ContainsLetters_ReturnsFalse()
        {
            var result = ValidationHelper.IsValidINN("123456789A");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidINN_EmptyString_ReturnsTrue()
        {
            var result = ValidationHelper.IsValidINN("");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidINN_Null_ReturnsTrue()
        {
            var result = ValidationHelper.IsValidINN(null);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidPhone_ValidPhoneWithPlus_ReturnsTrue()
        {
            var result = ValidationHelper.IsValidPhone("+7 (342) 123-45-67");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidPhone_ValidPhoneDigitsOnly_ReturnsTrue()
        {
            var result = ValidationHelper.IsValidPhone("89123456789");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidPhone_InvalidCharacters_ReturnsFalse()
        {
            var result = ValidationHelper.IsValidPhone("abc123");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidPhone_EmptyString_ReturnsTrue()
        {
            var result = ValidationHelper.IsValidPhone("");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidEmail_ValidEmail_ReturnsTrue()
        {
            var result = ValidationHelper.IsValidEmail("test@example.com");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidEmail_NoAtSymbol_ReturnsFalse()
        {
            var result = ValidationHelper.IsValidEmail("testexample.com");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidEmail_NoDomain_ReturnsFalse()
        {
            var result = ValidationHelper.IsValidEmail("test@");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidEmail_NoName_ReturnsFalse()
        {
            var result = ValidationHelper.IsValidEmail("@example.com");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidEmail_EmptyString_ReturnsTrue()
        {
            var result = ValidationHelper.IsValidEmail("");
            Assert.IsTrue(result);
        }
    }
}

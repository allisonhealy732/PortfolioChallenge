using System;
using CodingChallenge;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CodingChallenge_Tests
{
    [TestClass]
    public class ExchangeRates_Tests
    {
        private Mock<IServiceWrapper> mockServiceWrapper = new Mock<IServiceWrapper>();

        [TestInitialize]
        public void TestInit()
        {
            //Set the exchange rate to always be 3
            mockServiceWrapper.Setup(m => m.GetRate(It.IsAny<double>(), "EUR")).Returns(3);
            mockServiceWrapper.Setup(m => m.GetRate(It.IsAny<double>(), "USD")).Returns(1);
            mockServiceWrapper.Setup(m => m.IsValidSymbol(It.IsAny<string>())).Returns(true);
        }

        [TestMethod, TestCategory("UnitTest")]
        public void ExchangeRates_ConvertValue_UnitTest()
        {
            var exchangeRates = new ExchangeRates(mockServiceWrapper.Object);
            var rate = exchangeRates.ConvertValue(100, "USD", "EUR");
            Assert.IsTrue(rate == 300);
        }

        [TestMethod, TestCategory("UnitTest")]
        public void ExchangeRates_ConvertAmountToUSD_UnitTest()
        {
            var exchangeRates = new ExchangeRates(mockServiceWrapper.Object);
            var rate = exchangeRates.ConvertValue(100, "USD", "EUR");
            Assert.IsTrue(rate == 300);
        }
    }
}

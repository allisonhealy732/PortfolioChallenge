using System.Collections.Generic;
using CodingChallenge;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CodingChallenge_Tests
{
    [TestClass]
    public class AssetPortfolio_Tests
    {
        private Mock<IExchangeRates> mockExchangeRates { get; set; }

        [TestInitialize]
        public void TestInit()
        {
            mockExchangeRates = new Mock<IExchangeRates>();
        }

        [TestMethod, TestCategory("LiveTest")] //this is a live test because it hits the service directly
        public void Value_LiveTest()
        {
            var assetPortfolio = new AssetPortfolio(new ExchangeRates(new ServiceWrapper()))
            {
                Portfolio = new List<IAsset>()
                {
                    new Stock("ABC", "USD", 3, 2), //6 USD or 5ish GBP
                    new Stock("XYZ", "USD", 5, 1), //5 USD or 4ish GBP
                    new Currency("EUR", 100), //112 ish USD or 90ish GBP
                    new Currency("USD", 100), // 100 USD or 80ish GBP
                }
            };
            var value = assetPortfolio.Value("USD"); // should be 223 ish 

            Assert.IsFalse(value == 0);
            Assert.IsFalse(value == (6 + 5 + 100 + 100));

            value = assetPortfolio.Value("GBP"); //should be 179ish GBP

            Assert.IsFalse(value == 0);
            Assert.IsFalse(value == (6 + 5 + 100 + 100));
        }

        [TestMethod, TestCategory("UnitTest")]
        public void Value_UnitTest()
        {
            mockExchangeRates.Setup(m => m.ConvertValue(It.IsAny<double>(), It.IsAny<string>(), It.IsAny<string>())).Returns(3);
            var assetPortfolio = new AssetPortfolio(mockExchangeRates.Object);
            var value = assetPortfolio.Value("USD");

            Assert.AreEqual(value, 0);

            assetPortfolio.Portfolio = new List<IAsset>()
            {
                new Stock("ABC", "USD", 3, 2),
                new Stock("XYZ", "USD", 5, 1),
                new Currency("EUR", 100),
                new Currency("USD", 100),
            };

            value = assetPortfolio.Value("USD"); 

            Assert.AreEqual(value, 12);
        }


        [TestMethod, TestCategory("UnitTest")]
        public void Consolidate_UnitTest()
        {
            var assetPortfolio = new AssetPortfolio(mockExchangeRates.Object);
            assetPortfolio.Consolidate();
            Assert.AreEqual(assetPortfolio.ConsolidatedPortfolio.Count, 0);

            assetPortfolio.Portfolio = new List<IAsset>()
            {
                new Stock("ABC", "USD", 3, 2),
                new Stock("ABC", "USD", 5, 1),
                new Stock("ABC", "EUR", 10, 2),
                new Currency("EUR", 100),
                new Currency("EUR", 100),
                new Currency("USD", 300),
            };
            

            assetPortfolio.Consolidate();
            Assert.AreEqual(assetPortfolio.ConsolidatedPortfolio.Count, 4);
            Assert.IsTrue(assetPortfolio.ConsolidatedPortfolio["ABCUSD"].Shares == 8 && ((Stock)assetPortfolio.ConsolidatedPortfolio["ABCUSD"]).Price == 1.375);
            Assert.IsTrue(assetPortfolio.ConsolidatedPortfolio["ABCEUR"].Shares == 10 && ((Stock)assetPortfolio.ConsolidatedPortfolio["ABCEUR"]).Price == 2);
            Assert.AreEqual(assetPortfolio.ConsolidatedPortfolio["CashEUR"].Shares, 200);
            Assert.AreEqual(assetPortfolio.ConsolidatedPortfolio["CashUSD"].Shares, 300);
        }

        [TestMethod, TestCategory("UnitTest")]
        public void Consolidate_UnitTest_GivenExample()
        {
            var assetPortfolio = new AssetPortfolio(mockExchangeRates.Object)
            {
                Portfolio = new List<IAsset>()
                {
                    new Stock("ABC", "USD", 100, 2),
                    new Stock("ABC", "USD", 200, 3.5),
                    new Currency("EUR", 1000),
                    new Currency("EUR", 200),
                }
            };

            assetPortfolio.Consolidate();
            Assert.IsTrue(assetPortfolio.ConsolidatedPortfolio.Count == 2);
            Assert.IsTrue(assetPortfolio.ConsolidatedPortfolio["ABCUSD"].Shares == 300 && ((Stock)assetPortfolio.ConsolidatedPortfolio["ABCUSD"]).Price == 3);
            Assert.IsTrue(assetPortfolio.ConsolidatedPortfolio["CashEUR"].Shares == 1200);
        }

        

    }
}

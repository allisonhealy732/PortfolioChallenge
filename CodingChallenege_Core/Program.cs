using System;
using System.Collections.Generic;
using System.Linq;

namespace CodingChallenge
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Starting ...");
               // Test1();
                Test2();

                Console.WriteLine("Done... (Press a key to close)");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }

        private static void Test1()
        {
            var portfolio = new AssetPortfolio();
            portfolio.Add(new Stock("ABC", "USD", 200, 4));
            portfolio.Add(new Stock("DDW", "USD", 100, 10));
            Assert(AreEqual(portfolio.Value("USD"), 1800), " Test1 Failed, Expected Value:" + "\t" + 1800 + ",\t" + "Actual Value: \t" + portfolio.Value("USD") + "\n");
        }

        private static void Test2()
        {
            var assetPortfolio = new AssetPortfolio()
            {
                Portfolio = new List<IAsset>()
                {
                    new Stock("ABC", "USD", 3, 2),
                    new Stock("ABC", "USD", 5, 1),
                    new Stock("ABC", "EUR", 10, 2),
                    new Currency("EUR", 100),
                    new Currency("EUR", 100),
                    new Currency("USD", 300),
                }
            };
                
            PrintPortfolio(assetPortfolio.Portfolio, "Your initial Portfolio: ");
            assetPortfolio.Consolidate();
            PrintPortfolio(assetPortfolio.ConsolidatedPortfolio.Values.ToList(), "Your consolidated portfolio: ");

            Console.WriteLine($"The value of your portfolio in USD is: {string.Format("{0:N2}", assetPortfolio.Value("USD"))}");
            Console.WriteLine($"The value of your portfolio in GBP is: {string.Format("{0:N2}", assetPortfolio.Value("GBP"))}");
            Console.WriteLine($"The value of your portfolio in EUR is: {string.Format("{0:N2}", assetPortfolio.Value("EUR"))}");
        }

        private static void PrintPortfolio(List<IAsset> assets, string message)
        {
            Console.WriteLine(message);
            foreach (var asset in assets)
            {
                if (asset.GetType().Equals(typeof(Stock)))
                    Console.WriteLine($"Stock of {asset.Symbol} with {asset.Shares} shares at {((Stock)asset).Price} {asset.CurrencyType}");
                else
                    Console.WriteLine($"Currency of {asset.Shares} {asset.CurrencyType}");
            }
            Console.WriteLine();
        }

        private static void Assert(bool condition, string failure)
        {
            if (!condition)
                Console.WriteLine(failure);
        }

        private static bool AreEqual(double d1, double d2)
        {
            return Math.Abs(d1 - d2) < .0001;
        }

    }
}
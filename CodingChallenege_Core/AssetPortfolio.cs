using System;
using System.Linq;
using System.Collections.Generic;

namespace CodingChallenge
{
    public class AssetPortfolio
    {
        private IExchangeRates ExchangeRates { get; set; }
        public List<IAsset> Portfolio { get; set; }
        public Dictionary<string, IAsset> ConsolidatedPortfolio { get; set; }

        public AssetPortfolio() : this(new ExchangeRates()) { }
        public AssetPortfolio(IExchangeRates exchangeRates)
        {
            ExchangeRates = exchangeRates;
            Portfolio = new List<IAsset>();
            ConsolidatedPortfolio = new Dictionary<string, IAsset>();
        }

        public void Add(IAsset s)
        {
            Portfolio.Add(s);
        }

        public double Value(string identifier)
        {
            return Portfolio.Sum(asset => ExchangeRates.ConvertValue(asset.Value(), asset.CurrencyType, identifier));
        }

        public void Consolidate()
        {            
            foreach (var asset in Portfolio)
            {
                //the key is an aggregate so that Stock ABC with currency type USD and a stock ABC with currency type GBP remains seperate
                var key = $"{asset.Symbol}{asset.CurrencyType}";
                IAsset newAsset;

                if (ConsolidatedPortfolio.ContainsKey(key))
                {
                    var aggregatedAsset = ConsolidatedPortfolio[key];
                    var totalShares = asset.Shares + aggregatedAsset.Shares;

                    newAsset = asset.Symbol == "Cash" ? (IAsset)new Currency(asset.CurrencyType, totalShares) :
                                                        (IAsset)new Stock(asset.Symbol, asset.CurrencyType, totalShares, (aggregatedAsset.Value() + asset.Value())/totalShares);                   
                }
                else
                {
                    newAsset = asset;
                }

                ConsolidatedPortfolio[key] = newAsset;
            }
        }

    }
}
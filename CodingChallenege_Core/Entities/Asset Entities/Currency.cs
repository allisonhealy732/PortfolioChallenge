using System;

namespace CodingChallenge
{
    public class Currency : IAsset
    {
        public string CurrencyType { get; set; }
        public string Symbol { get; set; }
        public double Shares { get; set; }

        public Currency(string currencyType, double shares)
        {
            CurrencyType = currencyType;
            Shares = shares;
            Symbol = "Cash";
        }

        public double Value()
        {
            return Shares;
        }
    }
}

using System;

namespace CodingChallenge
{

    public interface IAsset
    {
        public string CurrencyType { get; set; }
        public string Symbol { get; set; }
        public double Shares { get; set; }

        public double Value();
    }
}

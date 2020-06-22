namespace CodingChallenge
{
    public class Stock : IAsset
    {
        public string CurrencyType { get; set; }
        public string Symbol { get; set; }
        public double Shares { get; set; }
        public double Price { get; set; }

        public Stock(string symbol, string currencyType, double shares, double price)
        {
            CurrencyType = currencyType;
            Symbol = symbol;
            Shares = shares;
            Price = price;
        }

        public double Value()
        {
            return Shares * Price;
        }
    }
}
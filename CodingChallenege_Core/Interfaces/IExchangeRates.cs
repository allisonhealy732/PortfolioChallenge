namespace CodingChallenge
{
    public interface IExchangeRates
    {
        double ConvertValue(double amount, string fromCurrency, string toCurrency);
    }
}
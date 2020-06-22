namespace CodingChallenge
{
    public interface IServiceWrapper
    {
        double GetRate(double amount, string toCurrency);
        bool IsValidSymbol(string symbol);
    }
}
using System;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodingChallenge
{
    public class ExchangeRates : IExchangeRates
    {
        //For this Implementation I had to convert to USD first due to limitations with the free version of the API.
        //I would do a straight conversion between currencies under normal circumstances.
        //Making two seperate service calls for this sort of thing is not ideal.

        private IServiceWrapper ServiceWrapper { get; set; }


        public ExchangeRates() : this(new ServiceWrapper()) { }
        public ExchangeRates(IServiceWrapper serviceWrapper)
        {
            ServiceWrapper = serviceWrapper;
        }

        public double ConvertValue(double amount, string fromCurrency, string toCurrency)
        {
            Validate(fromCurrency, toCurrency);

            var exchangeRateToNewCurrency = ServiceWrapper.GetRate(amount, toCurrency); // this will give exhange rate from USD to another currency
            return ConvertAmountToUSD(amount, fromCurrency) * exchangeRateToNewCurrency;
        }


        private double ConvertAmountToUSD(double amount, string fromCurrency)
        {
            //All exhance rates provided by this API are to go for USD to another currency. Dividing by 1 allows us to go back to USD from a different currency. 
            var exchangeRate = 1 / ServiceWrapper.GetRate(amount, fromCurrency); 
            return (amount * exchangeRate);
        }

        private void Validate(string fromCurrency, string toCurrency)
        {
            if (!ServiceWrapper.IsValidSymbol(fromCurrency)) 
                throw new SymbolValidationException(fromCurrency);
            else if (!ServiceWrapper.IsValidSymbol(toCurrency))
                throw new SymbolValidationException(toCurrency);
        }

    }
}

using System;
namespace CodingChallenge
{
    public class CurrencyLayerApiException : Exception
    {
        public CurrencyLayerApiException(string message)
            :base(message)
        {
        }
    }
}

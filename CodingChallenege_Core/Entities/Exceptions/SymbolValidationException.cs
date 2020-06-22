using System;
namespace CodingChallenge
{
    public class SymbolValidationException : Exception
    {
        public SymbolValidationException(string symbol)
            :base($"Validation Error: {symbol} is not a valid Currency Symbol"){ }

    }
}

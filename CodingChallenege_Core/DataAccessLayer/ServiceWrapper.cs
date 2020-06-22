using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CodingChallenge
{
    public class ServiceWrapper : IServiceWrapper
    {
        //The free version of this api only allows 250 requests per month. If this occurs, access key needs to be updated to next one in the list 
        private string AccessKey { get; set; } 
        private Dictionary<string, string> ValidSymbols { get; set; }
        private List<string> AccessKeys = new List<string>() { "6f7a5cade8c0144e35a22094dca4b6f2", "c70e0f046ea6b273c6c3d508023c1750", "8484b2ccb8d49b880559d801e71d1b55" };


        public ServiceWrapper()
        {
            AccessKey = AccessKeys[1];
            ValidSymbols = GetValidSymbols().Result; // Read the Valid Symbols into a Dictionary in the contructor so the API call only has to occur once
        }

        public double GetRate(double amount, string toCurrency) => GetRateFromApi(amount, toCurrency).Result;
        public bool IsValidSymbol(string symbol) => ValidSymbols.ContainsKey($"USD{symbol}");

        private async Task<double> GetRateFromApi(double amount, string toCurrency)
        {
            //var url = $"http://api.currencylayer.com/live?access_key={AccessKey}&currencies={toCurrency}";

            //string result = "";
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync($"http://api.currencylayer.com/live?access_key={AccessKey}&currencies={toCurrency}"))
                {
                    var result = response.Content.ReadAsStringAsync().Result;

                    var match = Regex.Match(result, $"\"USD{toCurrency}\":(\\d+(\\.\\d+)?)|(\\.\\d+)").Groups[1].Value;
                    return double.Parse(match);
                }
            }
        }

        private async Task<Dictionary<string, string>> GetValidSymbols()
        {
            //var url = $"http://api.currencylayer.com/live?access_key={AccessKey}";

            //string result = "";
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync($"http://api.currencylayer.com/live?access_key={AccessKey}"))
                {
                    var result = response.Content.ReadAsStringAsync().Result;

                    var quotes = JObject.Parse(result)["quotes"];

                    if (quotes == null)
                    {
                        var error = JObject.Parse(result)["error"];
                        var errorMessage = error.ToObject<Dictionary<string, string>>()["info"];
                        throw new CurrencyLayerApiException($"Error communicating with the Currency Layer Api: {errorMessage}");
                    }
                    return quotes.ToObject<Dictionary<string, string>>();

                }
            }
           
        }
    }
}

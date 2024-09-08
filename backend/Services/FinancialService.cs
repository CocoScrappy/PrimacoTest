using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace backend.Services
{
    public class FinancialService
    {
        public async Task<string> GetFinancialData(string ticker)
        {
            var client = new HttpClient();
            var response = await client.GetStringAsync($"https://www.alphavantage.co/query?function=OVERVIEW&symbol={ticker}&apikey={Environment.GetEnvironmentVariable("ALPHA_VANTAGE_API")}");
            var json = JObject.Parse(response);
            var companyData = new
            {
                Name = json["Name"]?.ToString(),
                MarketCap = json["MarketCapitalization"]?.ToString(),
                PERatio = json["PERatio"]?.ToObject<decimal>(),
                Sector = json["Sector"]?.ToString(),
                Industry = json["Industry"]?.ToString(),
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(companyData);
        }
    }
}
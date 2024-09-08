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
                Industry = json["Industry"]?.ToString(),
                Sector = json["Sector"]?.ToString(),
                MarketCapitalization = json["MarketCapitalization"]?.ToString(),
                PERatio = json["PERatio"]?.ToObject<decimal>()
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(companyData);
        }
    }
}
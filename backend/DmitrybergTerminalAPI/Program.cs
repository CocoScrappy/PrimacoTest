using System.Net;
using Newtonsoft.Json.Linq;
using DmitrybergTerminalAPI.Data; 

namespace DmitrybergTerminalAPI
{
    class Program
    {
        private static readonly string ConnectionString = "Your_Connection_String_Here";
        
        static async Task Main(string[] args)
        {
            using var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                eventArgs.Cancel = true;
                cts.Cancel();
            };

            var listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:5000/");
            listener.Start();
            Console.WriteLine("Listening for connections on http://localhost:5000/");

            // Instantiate DatabaseService
            var databaseService = new DatabaseService(ConnectionString);

            try
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    try
                    {
                        var context = await listener.GetContextAsync();
                        var response = context.Response;
                        string responseString;

                        var request = context.Request;

                        if (request.Url.AbsolutePath.StartsWith("/api/search"))
                        {
                            var ticker = request.Url.Segments.Last();
                            if (!string.IsNullOrWhiteSpace(ticker))
                            {
                                var financialData = await GetFinancialData(ticker);
                                await databaseService.InsertSearchHistory(1, ticker); // UserId hardcoded for simplicity
                                responseString = financialData;
                            }
                            else
                            {
                                responseString = "Ticker symbol cannot be empty.";
                                response.StatusCode = (int)HttpStatusCode.BadRequest;
                            }
                        }
                        else
                        {
                            responseString = "Endpoint not found.";
                            response.StatusCode = (int)HttpStatusCode.NotFound;
                        }

                        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                        response.ContentLength64 = buffer.Length;
                        await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }
            finally
            {
                listener.Stop();
                Console.WriteLine("Server stopped.");
            }
        }

        private static async Task<string> GetFinancialData(string ticker)
        {
            var client = new HttpClient();
            var response = await client.GetStringAsync($"https://finance.yahoo.com/quote/{ticker}");
            var json = JObject.Parse(response);
            var companyData = new
            {
                Name = json["quoteType"]?["shortName"]?.ToString(),
                Price = json["price"]?["regularMarketPrice"]?["raw"]?.ToObject<decimal>(),
                MarketCap = json["summaryDetail"]?["marketCap"]?["raw"]?.ToObject<decimal>(),
                PERatio = json["summaryDetail"]?["trailingPE"]?["raw"]?.ToObject<decimal>(),
                Industry = json["summaryProfile"]?["industry"]?.ToString()
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(companyData);
        }
    }
}

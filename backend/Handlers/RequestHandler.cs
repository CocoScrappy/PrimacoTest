using System;
using System.Net;
using System.Threading.Tasks;
using backend.Helpers;
using backend.Services;
using System.Linq;


namespace backend.Handlers
{
    public static class RequestHandler
    {
        public static async Task HandleRequest(HttpListenerContext context, DatabaseService databaseService, FinancialService financialService)
            {
                var response = context.Response;
                string responseString;

                var request = context.Request;
                if (request.Url.AbsolutePath.StartsWith("/api/search"))
                {
                    var ticker = request.Url.Segments.Last();
                    if (!string.IsNullOrWhiteSpace(ticker))
                    {
                        var financialData = await financialService.GetFinancialData(ticker);
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

                await HttpHelper.WriteResponse(response, responseString);
            }
    }
}
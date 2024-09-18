using System.Net;
using Newtonsoft.Json.Linq;
using backend.Services;
using backend.Helpers;
using backend.Handlers;
using DotNetEnv;
using System.Threading.Tasks;

namespace backend
{
    class Program
    {        
        static async Task Main(string[] args)
        {
            Env.Load("../.env");
            using var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                eventArgs.Cancel = false;  // Allow natural termination after cleanup
                cts.Cancel();
            };

            using var listener = ServerHelper.StartHttpListener("http://localhost:5000/");

            var connectionString = ConfigHelper.GetConnectionString();
            if (string.IsNullOrWhiteSpace(connectionString)) return;

            var databaseService = new DatabaseService(connectionString);
            var financialService = new FinancialService();

            await ServerService.RunServer(listener, databaseService, financialService, cts.Token);
        }
    }
}

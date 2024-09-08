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

            // Start the HTTP listener
            using var listener = StartHttpListener();

            // Get connection string from environment variable
            var connectionString = GetConnectionString();
            if (string.IsNullOrWhiteSpace(connectionString)) return;

            var databaseService = new DatabaseService(connectionString);
            var financialService = new FinancialService();

            await RunServer(listener, databaseService, financialService, cts.Token);
        }

        private static HttpListener StartHttpListener()
        {
            var listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:5000/");
            listener.Start();
            Console.WriteLine("Listening for connections on http://localhost:5000/");
            return listener;
        }

        private static string GetConnectionString()
        {
            var connectionString = Environment.GetEnvironmentVariable("MSSQL_CONNECTION_STRING");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                Console.WriteLine("Connection string not found. Exiting...");
                return null;
            }
            Console.WriteLine("Connection string found. Connection string: " + connectionString);
            return connectionString;
        }

        private static async Task RunServer(HttpListener listener, DatabaseService databaseService, FinancialService financialService, CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var contextTask = listener.GetContextAsync();
                    var completedTask = await Task.WhenAny(contextTask, Task.Delay(-1, token));

                    if (completedTask == contextTask)
                    {
                        var context = await contextTask;
                        await RequestHandler.HandleRequest(context, databaseService, financialService);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Server stopped because of cancellation.");
            }
            finally
            {
                listener.Stop();
                listener.Close();  // Ensure that resources are properly cleaned up
                Console.WriteLine("Server stopped.");
            }
        }
    }
}

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using backend.Services;
using backend.Handlers;

public class ServerService
{
    public static async Task RunServer(HttpListener listener, DatabaseService databaseService, FinancialService financialService, CancellationToken token)
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
            listener.Close();
            Console.WriteLine("Server stopped.");
        }
    }
}

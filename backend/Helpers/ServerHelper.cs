using System;
using System.Net;

public static class ServerHelper
{
    public static HttpListener StartHttpListener(string url)
    {
        var listener = new HttpListener();
        listener.Prefixes.Add(url);
        listener.Start();
        Console.WriteLine($"Listening for connections on {url}");
        return listener;
    }
}

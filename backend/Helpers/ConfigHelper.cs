using System;

public static class ConfigHelper
{
    public static string GetConnectionString()
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
}

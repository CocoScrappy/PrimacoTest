using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace backend.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(string connectionString)
        {
            _connectionString = connectionString;
            
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

public async Task InsertSearchHistory(int userId, string tickerSymbol, decimal? PERatio, string MarketCap, string Sector, string Industry, string Name)
{
    using var connection = GetConnection();
    await connection.OpenAsync();
    var command = connection.CreateCommand();
    command.CommandText = "INSERT INTO SearchHistory (UserId, TickerSymbol, SearchDate, PERatio, MarketCap, Sector, Industry, Name) VALUES (@userId, @tickerSymbol, @searchDate, @PERatio, @MarketCap, @Sector, @Industry, @Name)";
    command.Parameters.AddWithValue("@userId", userId);
    command.Parameters.AddWithValue("@tickerSymbol", tickerSymbol);
    command.Parameters.AddWithValue("@searchDate", DateTime.Now);
    command.Parameters.AddWithValue("@PERatio", (object)PERatio ?? DBNull.Value);
    command.Parameters.AddWithValue("@MarketCap", (object)MarketCap ?? DBNull.Value);
    command.Parameters.AddWithValue("@Sector", (object)Sector ?? DBNull.Value);
    command.Parameters.AddWithValue("@Industry", (object)Industry ?? DBNull.Value);
    command.Parameters.AddWithValue("@Name", (object)Name ?? DBNull.Value);
    await command.ExecuteNonQueryAsync();
}


        //GetLatestSearchHistory ticker and PERatio
public async Task<object> GetLatestSearchHistory(int userId)
{
    using var connection = GetConnection();
    await connection.OpenAsync();
    var command = connection.CreateCommand();
    command.CommandText = "SELECT TOP 1 TickerSymbol, PERatio, MarketCap, Sector, Industry, Name, SearchDate FROM SearchHistory WHERE UserId = @userId ORDER BY SearchDate DESC";
    command.Parameters.AddWithValue("@userId", userId);
    var reader = await command.ExecuteReaderAsync();
    
    if (await reader.ReadAsync())
    {
        return new
        {
            TickerSymbol = reader.GetString(0),
            PERatio = reader.IsDBNull(1) ? (decimal?)null : reader.GetDecimal(1), 
            MarketCap = reader.IsDBNull(2) ? (decimal?)null : reader.GetDecimal(2), 
            Sector = reader.IsDBNull(3) ? null : reader.GetString(3), 
            Industry = reader.IsDBNull(4) ? null : reader.GetString(4), 
            Name = reader.IsDBNull(5) ? null : reader.GetString(5),
            SearchDate = reader.GetDateTime(6)
        };
    }

    return null;
}



    }
}

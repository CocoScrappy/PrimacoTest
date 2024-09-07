using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DmitrybergTerminalAPI.Data
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

        public async Task InsertSearchHistory(int userId, string tickerSymbol)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();
            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO SearchHistory (UserId, TickerSymbol, SearchDate) VALUES (@userId, @tickerSymbol, @searchDate)";
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@tickerSymbol", tickerSymbol);
            command.Parameters.AddWithValue("@searchDate", DateTime.Now);
            await command.ExecuteNonQueryAsync();
        }
    }
}
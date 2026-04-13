using Npgsql;


namespace Persistence.Context
{
    public class DbProvider
    {
        private readonly string _connectionString;

        public DbProvider(string connectionString) { _connectionString = connectionString; }

        public NpgsqlConnection GetConnection() => new NpgsqlConnection(_connectionString);

    }
}

using Persistence.Context;
using Npgsql;
using System.Data.Common;

namespace Persistence.Repository
{
    public class PostRepository: IPostRepository
    {
        private readonly DbProvider _provider;

        public PostRepository(DbProvider provider) => _provider = provider;
   
        public async Task PostPost(int Id)
        {
            using var connection = _provider.GetConnection();
            await connection.OpenAsync();

            var sql = "INSERT INTO post(Id, Text) VALUES(@Id, @Text)";

            using var command = new NpgsqlCommand(sql, connection);


            // write unit test for that method. For check how does it work


            command.Parameters.AddWithValue("Id", Id);
            //command.Parameters.AddWithValue("@params", specific my value from another code);

            await command.ExecuteNonQueryAsync();
        }
    }
}

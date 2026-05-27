using Core.Interfaces;
using Core.Models;
using Npgsql;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence
{
    public class PersistanceCore: IPersistanseCore
    {
        private readonly DbProvider _provider;

        public PersistanceCore(DbProvider provider) { _provider = provider; }

        public async Task<User> GetUserDataById(Guid id) // if its universal method - add new sql command for geting user's posts
        {                                                // maybe the implementaton of that method is not good idea because of the method get all user's data for one thing like 'AuthorName'. Maybe!!!
            using var connection = _provider.GetConnection();

            await connection.OpenAsync();

            var sql = """
                SELECT * FROM users
                WHERE id = @id 
                """;

            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("id", id);

            using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync()) { Console.WriteLine("There is no rows was found"); }

            User user = new User();

            user.SetId(reader.GetGuid(reader.GetOrdinal("id"))); // its better solution use GetOrdnal. Remake outher mathods.
            user.SetPassword(reader.GetString(reader.GetOrdinal("password")));
            user.Name = reader.GetString(reader.GetOrdinal("name"));

            return user;
        }
    }
}

using Core.Interfaces;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Text;
using Core.Models;
using Npgsql;

namespace Persistence.Repository
{
    public class UserRepository: IUserReposytory
    {
        private readonly DbProvider _provider;

        public UserRepository(DbProvider provider) { _provider = provider; }

        public async Task<User> SignUp(User user)
        {
            using var connection = _provider.GetConnection();

            await connection.OpenAsync();

            var sql = @"INSERT INTO users (id, name, password)
                        VALUES(@Id, @Name, @Password)";

            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("Id", user.Id);
            command.Parameters.AddWithValue("Name", user.Name);
            command.Parameters.AddWithValue("Password", user.Password);

            int rowsExecuted = await command.ExecuteNonQueryAsync();

            if (rowsExecuted > 0) { return user; }

            throw new Exception("Error when \'Create new User\' request was executed");
        }
    }
}

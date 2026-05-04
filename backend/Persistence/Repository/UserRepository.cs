using Core.Interfaces;
using Core.Models;
using Npgsql;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Persistence.Repository
{
    public class UserRepository: IUserReposytory
    {
        private readonly DbProvider _provider;

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

        public async Task<User> SignIn(User user)
        {
            using var connection = _provider.GetConnection();
            await connection.OpenAsync();

            var sql = @"SELECT 
                              id, 
                              name,
                              password
                              FROM users
                              WHERE name = @name";

            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("name", user.Name);

            using var reader = await command.ExecuteReaderAsync();

            await reader.ReadAsync();

            User newUser = new User();
            newUser.SetId(reader.GetGuid(0));
            newUser.Name = reader.GetString(1);
            newUser.SetPassword(reader.GetString(2));

            return newUser;
        }


        public UserRepository(DbProvider provider) { _provider = provider; }
    }
}

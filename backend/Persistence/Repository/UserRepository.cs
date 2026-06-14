using Core.Interfaces;
using Core.Models;
using Npgsql;
using Persistence.Context;

namespace Persistence.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly DbProvider _provider;
        private IPersistanseCore _persistanseCore;

        public UserRepository(DbProvider provider, IPersistanseCore persistacseCore) { _provider = provider; _persistanseCore = persistacseCore; }

        public async Task<User> SignUp(User user)
        {
            using var connection = _provider.GetConnection();

            await connection.OpenAsync();

            var sql = @"INSERT INTO users (id, name, password)
                        VALUES(@PostId, @Name, @Password)";

            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("PostId", user.Id);
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

            if (!await reader.ReadAsync()) { Console.WriteLine("There is no rows was found"); }

            User newUser = new User();
            newUser.SetId(reader.GetGuid(reader.GetOrdinal("id")));
            newUser.Name = reader.GetString(1);
            newUser.SetPassword(reader.GetString(2));

            return newUser;
        }
        public async Task<User> GetUserDataById(Guid id) // if its universal method - add new sql command for geting user's posts
        {                                                // maybe the implementaton of that method is not good idea because of the method get all user's data for one thing like 'AuthorName'. Maybe!!!
            User user = await _persistanseCore.GetUserDataById(id);
            return user;

        }







       
    }
}

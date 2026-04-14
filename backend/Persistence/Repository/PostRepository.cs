using Persistence.Context;
using Npgsql;
using System.Data.Common;
using Core.Models;

namespace Persistence.Repository
{
    public class PostRepository: IPostRepository
    {
        private readonly DbProvider _provider;

        public PostRepository(DbProvider provider) => _provider = provider;
   
        /*
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
        } */

        public async Task<List<Post>> GetAllPosts() 
        {
            using var connection = _provider.GetConnection();
            await connection.OpenAsync();

            var sql = @"
                SELECT
                    id, 
                    author_name,
                    content,
                    created_at,
                    reaction_count
                    complaint_count
                    FROM posts
                    ORDER BY created_at DESC,
                    LIMIT 10";

            using var command = new NpgsqlCommand(sql, connection);

            List<Post> posts = new();

            using var reader = await command.ExecuteReaderAsync();


            while(await reader.ReadAsync())
            {
                var post = new Post();

                post.SetId(reader.GetGuid(0));
                post.AuthorName = reader.GetString(1);
                post.Content = reader.GetString(2);
                post.SetDateCreatedAt(reader.GetDateTime(3));
                post.SetReactionCount(reader.GetInt32(4));
                post.SetComplaintCount(reader.GetInt32(5));

                posts.Add(post);
            }

            return posts;
        }
    }
}


//SELECT id AS Id, post_text AS Text FROM post
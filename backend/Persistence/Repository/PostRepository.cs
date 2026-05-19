using Persistence.Context;
using Npgsql;
using System.Data.Common;
using Core.Models;

namespace Persistence.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly DbProvider _provider;

        public PostRepository(DbProvider provider) { _provider = provider; }

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
                    reaction_count,
                    complaint_count
                    FROM posts
                    ORDER BY created_at DESC
                    LIMIT 10";

            using var command = new NpgsqlCommand(sql, connection);

            List<Post> posts = new();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
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

        public async Task SavePost(Post post)
        {
            using var connection = _provider.GetConnection();

            await connection.OpenAsync();

            var sql = """
                INSERT INTO posts(id, author_name, content, created_at, reaction_count, complaint_count, user_id)
                VALUES(@id, @author_name, @content, @created_at, @reaction_count, @complaint_count, @user_id)
                """;

            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("id", post.Id);
            command.Parameters.AddWithValue("author_name", post.AuthorName);
            command.Parameters.AddWithValue("content", post.Content);
            command.Parameters.AddWithValue("created_at",post.CreatedAt);
            command.Parameters.AddWithValue("reaction_count", post.ReactionCount);
            command.Parameters.AddWithValue("complaint_count", post.ComplaintCount);
            command.Parameters.AddWithValue("user_id", post.UserId);

            await command.ExecuteNonQueryAsync();
        }
    
    }
}


//SELECT id AS Id, post_text AS Text FROM post
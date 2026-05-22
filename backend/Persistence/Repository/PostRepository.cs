using Core.Models;
using Npgsql;
using Persistence.Context;
using System.Data.Common;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

namespace Persistence.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly DbProvider _provider;

        public PostRepository(DbProvider provider) { _provider = provider; }

        public async Task<List<Post>> GetAllPostsAsync()
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

        public async Task<List<Like>> GetLikesListByIdAsync(Guid userId)
        {
            using var connection = _provider.GetConnection();

            await connection.OpenAsync();

            var sql = """
                SELECT 
                post_id
                FROM likes
                WHERE
                user_id = @user_id
                """;

            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("user_id", userId);

            using var reader = await command.ExecuteReaderAsync();

            List<Like> likesList = new List<Like>();

            while (await reader.ReadAsync())
            {
                Like like = new Like();

                like.userId = userId;
                like.postId = reader.GetGuid(reader.GetOrdinal("post_id"));

                likesList.Add(like);
            }

            return likesList;
        }

        public async Task SavePostAsync(Post post)
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

        public async Task SetLikeByPostIdANDUserIdAsync(Guid postId, Guid userId)
        {
            using var connection = _provider.GetConnection();

            await connection.OpenAsync();

            var sql = """
                INSERT INTO likes(post_id, user_id)
                VALUES (@post_id, @user_id)
                """;

            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("post_id", postId);
            command.Parameters.AddWithValue("user_id", userId);

            await command.ExecuteNonQueryAsync();

            sql = """
                UPDATE posts
                SET reaction_count = reaction_count + 1
                WHERE id = @postId
                """;

            using var newCommand = new NpgsqlCommand(sql, connection);

            newCommand.Parameters.AddWithValue("postId", postId);

            await newCommand.ExecuteNonQueryAsync();
        }

        public async Task RemoveLikeByPostIdANDUserIdAsync(Guid postId, Guid userId)
        {
            using var connection = _provider.GetConnection();

            await connection.OpenAsync();

            var sql = """
                DELETE FROM likes
                WHERE post_id = @post_id
                AND user_id = @user_id
                """;
            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("post_id", postId);
            command.Parameters.AddWithValue("user_id", userId);

            command.ExecuteNonQuery();

            sql = """
                UPDATE posts
                SET rection_count = reaction_count - 1
                WHERE id = @post_id
                AND user_id = @user_id
                """;

            using var newCommand = new NpgsqlCommand(sql, connection);

            newCommand.Parameters.AddWithValue("post_id", postId);
            newCommand.Parameters.AddWithValue("user_id", userId);

            await newCommand.ExecuteNonQueryAsync();
        }

        //public async Task DeletePostById(Guid postId) { }





        




     
    }
}

using Application.DTOs;
using Core.Interfaces;
using Core.Models;
using Npgsql;
using Persistence.Context;
using System.Data.Common;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

namespace Persistence.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly DbProvider _provider;
        private readonly IPersistanseCore _persistanseCore;
        public PostRepository(DbProvider provider, IPersistanseCore persistanceCore) { _provider = provider; _persistanseCore = persistanceCore; }

        public async Task<List<Post>> GetAllPostsAsync()
        {
            using var connection = _provider.GetConnection();

            await connection.OpenAsync();

            var sql = @"
                SELECT
                    id, 
                    user_id,
                    content,
                    created_at
                    FROM posts
                    ORDER BY created_at DESC
                    LIMIT 10";

            using var command = new NpgsqlCommand(sql, connection);

            using var reader = await command.ExecuteReaderAsync();


            List<(Guid userId, Guid postId)> ids = new();

            //Get posts
            List<Post> posts = new();

            while (await reader.ReadAsync())
            {
                var post = new Post();

                Guid postId = reader.GetGuid(0);
                Guid userId = reader.GetGuid(1);
                User currentUser = await _persistanseCore.GetUserDataById(userId);

                ids.Add((userId, postId));
                
                post.SetPostId(reader.GetGuid(0));
                post.SetUserId(reader.GetGuid(1));
                post.AuthorName = currentUser.Name;
                post.Content = reader.GetString(2);
                post.SetDateCreatedAt(reader.GetDateTime(3));

                posts.Add(post);
            }

            await reader.CloseAsync();

            //Add likes and complaints count
            for (int i = 0; i < posts.Count; i++)
            {
                sql = """
                SELECT 
                COUNT(*)
                FROM posts_likes
                WHERE post_id = @post_id
                """;

                using var newCommand = new NpgsqlCommand(sql, connection);

                newCommand.Parameters.AddWithValue("post_id", ids[i].postId);

                using var newReader = await newCommand.ExecuteReaderAsync();

                while (await newReader.ReadAsync())
                {
                    long count = newReader.GetInt64(0);

                    if (count > 0)
                    {
                        for (int j = 0; j < posts.Count; j++)
                        {
                            if (ids[i].postId == posts[j].Id)
                            {
                                posts[j].SetReactionCount((int)count);
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < posts.Count; j++)
                        {
                            if (ids[i].postId == posts[j].Id)
                            {
                                posts[j].SetReactionCount(0);
                            }
                        }
                    }
                }
                await newReader.CloseAsync();
            }

            return posts;
        }

        public async Task<List<PostLike>> GetPostLikesListByIdAsync(Guid userId)
        {
            using var connection = _provider.GetConnection();

            await connection.OpenAsync();

            var sql = """
                SELECT 
                post_id
                FROM posts_likes
                WHERE
                user_id = @user_id
                """;

            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("user_id", userId);

            using var reader = await command.ExecuteReaderAsync();

            List<PostLike> likesList = new List<PostLike>();

            while (await reader.ReadAsync())
            {
                PostLike like = new PostLike();

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
                INSERT INTO posts(id, content, created_at, user_id)
                VALUES(@id, @content, @created_at, @user_id)
                """;

            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("id", post.Id);
            command.Parameters.AddWithValue("content", post.Content);
            command.Parameters.AddWithValue("created_at",post.CreatedAt);
            command.Parameters.AddWithValue("user_id", post.UserId);

            await command.ExecuteNonQueryAsync();
        }

        public async Task SetLikeByPostIdANDUserIdAsync(Guid postId, Guid userId)
        {
            using var connection = _provider.GetConnection();

            await connection.OpenAsync();

            var sql = """
                INSERT INTO posts_likes(post_id, user_id)
                VALUES (@post_id, @user_id)
                """;

            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("post_id", postId);
            command.Parameters.AddWithValue("user_id", userId);

            await command.ExecuteNonQueryAsync();
        }

        public async Task RemoveLikeByPostIdANDUserIdAsync(Guid postId, Guid userId)
        {
            using var connection = _provider.GetConnection();

            await connection.OpenAsync();

            //await using var transaction = await connection.BeginTransactionAsync(); // opening of transaction

            try
            {
                var sql = """
                 DELETE FROM posts_likes
                 WHERE post_id = @post_id
                 AND user_id = @user_id
                 """;
                using var deleteCommand = new NpgsqlCommand(sql, connection/*, transaction*/);

                deleteCommand.Parameters.AddWithValue("post_id", postId);
                deleteCommand.Parameters.AddWithValue("user_id", userId);

                await deleteCommand.ExecuteNonQueryAsync();

               // await transaction.CommitAsync();//if all are saccsesful it says to BD - save all reale
            }
            catch
            {
                //await transaction.RollbackAsync();// if thomething is failed - cansel all changes back
                throw;
            }
            
        }

        public async Task<int> DeletePost(Guid postId)
        {
            using var connection = _provider.GetConnection();

            await connection.OpenAsync();

            var sql = """
                DELETE FROM posts
                WHERE id = @postId
                """;

            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("postId", postId);

            int deletedRows = await command.ExecuteNonQueryAsync();

            return deletedRows;
        }

        public async Task<List<Post>> GetAllPostByUserIdAsync(Guid userId)
        {
            using var connection = _provider.GetConnection();

            await connection.OpenAsync();

            var sql = """
                SELECT 
                    id, 
                    user_id,
                    content,
                    created_at
                    FROM posts
                    WHERE user_id = @userId
                    ORDER BY created_at DESC
                """;

            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("userId", userId);

            using var reader = await command.ExecuteReaderAsync();

            List<Post> posts = new();

            while (await reader.ReadAsync())
            {
                Post post = new Post();

                User currentUser = await _persistanseCore.GetUserDataById(userId);

                post.SetPostId(reader.GetGuid(0));
                post.SetUserId(reader.GetGuid(1));
                post.AuthorName = currentUser.Name;
                post.Content = reader.GetString(2);
                post.SetDateCreatedAt(reader.GetDateTime(3));

                posts.Add(post);
            }

            return posts;
        }

        public async Task SetComplaint(Complaint complaint)
        {
            using var connection = _provider.GetConnection();

            await connection.OpenAsync();

            var sql = """
                INSERT INTO complaints(id, reason, user_id, post_id)
                VALUES(@id, @reason, @user_id, @post_id)
                """;

            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("id", complaint.ComplaintId);
            command.Parameters.AddWithValue("reason", complaint.Reason);
            command.Parameters.AddWithValue("user_id", complaint.UserId);
            command.Parameters.AddWithValue("post_id", complaint.PostId);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<(Post post, string reason)>> GetAllPostsWithComplaints()
        {
            using var connection = _provider.GetConnection();
            
            await connection.OpenAsync();

            var sql = """
                SELECT 
                posts.id,
                posts.content,
                posts.created_at,
                posts.user_id,
                complaints.reason
                FROM posts
                INNER JOIN complaints ON posts.id = complaints.post_id
                """;

            using var command = new NpgsqlCommand(sql, connection);

            using var reader = await command.ExecuteReaderAsync();

            List<(Post post, string reason)> complaintList = new List<(Post post, string reason)>();

            while (await reader.ReadAsync())
            {
                string reason = reader.GetString(reader.GetOrdinal("reason"));

                Post post = new Post();

                post.SetPostId(reader.GetGuid(reader.GetOrdinal("id")));
                post.Content = reader.GetString(reader.GetOrdinal("content"));
                post.SetDateCreatedAt(reader.GetDateTime(reader.GetOrdinal("created_at")));
                post.SetUserId(reader.GetGuid(reader.GetOrdinal("user_id")));

                complaintList.Add((post, reason));
            }

            return complaintList;
        }





    }
}

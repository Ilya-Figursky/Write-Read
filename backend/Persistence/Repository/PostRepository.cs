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
        public PostRepository(DbProvider provider, IPersistanseCore persistacseCore) { _provider = provider; _persistanseCore = persistacseCore; }

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
                
                post.SetId(reader.GetGuid(0));
                post.SetUserId(reader.GetGuid(1));
                post.AuthorName = currentUser.Name;
                post.Content = reader.GetString(2);
                post.SetDateCreatedAt(reader.GetDateTime(3));
                //post.SetReactionCount(reader.GetInt32(4));
                //post.SetComplaintCount(reader.GetInt32(5));

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

        public async Task<List<PostLike>> GetLikesListByIdAsync(Guid userId)
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


        //remake it
        public async Task SaveCommentAsync(Guid postId, Guid userId, string textContent)
        {
            using var connection = _provider.GetConnection();

            await connection.OpenAsync();

            var sql = """
                INSERT INTO comments(author_name, content, created_at, reaction_count, complaint_count, post_id, user_id)
                VALUES(@author_name, @content, @created_at, @reaction_count, @complaint_count, @post_id, @user_id)
                """;

            using var command = new NpgsqlCommand(sql, connection);

            User user = await _persistanseCore.GetUserDataById(userId);
            Comment comment = new(user.Name, textContent, userId);

            command.Parameters.AddWithValue("author_name", comment.AuthorName);
            command.Parameters.AddWithValue("content", comment.Content);
            command.Parameters.AddWithValue("created_at",comment.CreatedAt);
            command.Parameters.AddWithValue("reaction_count", comment.ReactionCount);
            command.Parameters.AddWithValue("complaint_count", comment.ReactionCount);
            command.Parameters.AddWithValue("post_id", comment.Id);
            command.Parameters.AddWithValue("user_id", comment.UserId);

            await command.ExecuteNonQueryAsync();
        }










    }
}

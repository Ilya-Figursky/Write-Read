using Application.DTOs;
using Core.Interfaces;
using Core.Models;
using Npgsql;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Persistence.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DbProvider _provider;
        private readonly IPersistanseCore _persistanceCore;

        public CommentRepository(DbProvider provider, IPersistanseCore persistanseCore) { _provider = provider; _persistanceCore = persistanseCore; }

        public async Task SaveCommentAsync(Comment comment)
        {
            using var connection = _provider.GetConnection();

            await connection.OpenAsync();

            var sql = """
                INSERT INTO comments(id, content, created_at, user_id, post_id)
                VALUES(@id, @content, @created_at, @user_id, @post_id)
                """;

            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("id", comment.Id);
            command.Parameters.AddWithValue("content", comment.Content);
            command.Parameters.AddWithValue("created_at", comment.CreatedAt);
            command.Parameters.AddWithValue("user_id", comment.UserId);
            command.Parameters.AddWithValue("post_id", comment.PostId);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<Comment>> GetAllCommentsByPostIdANDUserId(Guid postId, Guid userId)
        {
            using var connection = _provider.GetConnection();

            await connection.OpenAsync();

            var sql = """
                SELECT
                id,
                content,
                created_at,
                user_id,
                post_id
                FROM comments
                WHERE post_id = @postId
                """;

            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("postId", postId);

            using var reader = await command.ExecuteReaderAsync();

            List<Comment> comments = new List<Comment>();

            List<(Guid userId, Guid commentId)> ids = new List<(Guid userId, Guid commentId)>();

            while (await reader.ReadAsync())
            {
                var comment = new Comment();

                Guid userID = reader.GetGuid(reader.GetOrdinal("user_id"));

                User user = await _persistanceCore.GetUserDataById(userId);

                var commentID = reader.GetGuid(reader.GetOrdinal("id"));

                ids.Add((userID, commentID));

                comment.SetUserId(userID);
                comment.SetPostId(postId);
                comment.SetCommentId(reader.GetGuid(reader.GetOrdinal("id")));
                comment.Content = reader.GetString(reader.GetOrdinal("content"));
                comment.SetCreatedAtTime(reader.GetDateTime(reader.GetOrdinal("created_at")));

                comments.Add(comment);
            }

            await reader.CloseAsync();

            //Add likes and complaints count
            for (int i = 0; i < comments.Count; i++)
            {
                sql = """
                    SELECT 
                    COUNT(*)
                    FROM comments_likes
                    WHERE comment_id = @comment_id
                    """;

                using var newCommand = new NpgsqlCommand(sql, connection);

                newCommand.Parameters.AddWithValue("comment_id", ids[i].commentId);

                using var newReader = await newCommand.ExecuteReaderAsync();

                while (await newReader.ReadAsync())
                {
                    long count = newReader.GetInt64(0);

                    if (count > 0)
                    {
                        for (int j = 0; j < comments.Count; j++)
                        {
                            if (ids[i].commentId == comments[j].Id) { comments[j].SetReactionCount((int)count); }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < comments.Count; j++)
                        {
                            if (ids[i].commentId == comments[j].Id) { comments[j].SetReactionCount(0); }
                        }
                    }

                }
                await newReader.CloseAsync();
            }
            return comments;
        }

        public async Task<List<CommentLike>> GetCommentLikesListByUserIdPostId(Guid postId, Guid userId)
        {
            using var connection = _provider.GetConnection();

            await connection.OpenAsync();

            var sql = """
                SELECT 
                comment_id
                FROM comments_likes
                WHERE 
                user_id = @user_id
                """;

            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("user_id", userId);

            using var reader = await command.ExecuteReaderAsync();

            List<CommentLike> likesList = new List<CommentLike>();

            while (await reader.ReadAsync())
            {
                CommentLike commentLike = new CommentLike();

                commentLike.userId = userId;
                commentLike.commentId = reader.GetGuid(reader.GetOrdinal("comment_id"));

                likesList.Add(commentLike);
            }

            return likesList;
        }
            
        public async Task SaveCommentLikeAsync(Guid userId, Guid commentId)
        {
            using var connection = _provider.GetConnection();

            await connection.OpenAsync();

            var sql = """
                INSERT INTO comments_likes(user_id, comment_id)
                VALUES(@user_id, @comment_id)
                """;

            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("user_id", userId);
            command.Parameters.AddWithValue("comment_id", commentId);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteCommentLikeAsync(Guid userId, Guid commentId)
        {
            using var connection = _provider.GetConnection();

            await connection.OpenAsync();

            var sql = """
                DELETE FROM comments_likes
                WHERE user_id = @userId
                AND comment_id = @commentId
                """;

            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("userid", userId);
            command.Parameters.AddWithValue("commentid", commentId);

            await command.ExecuteNonQueryAsync();
        }

    }
}

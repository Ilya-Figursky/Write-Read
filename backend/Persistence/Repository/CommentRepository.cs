using Core.Interfaces;
using Core.Models;
using Npgsql;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DbProvider _provider;

        public CommentRepository(DbProvider provider) { _provider = provider; }

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
    }
}

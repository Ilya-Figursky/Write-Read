using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    public interface ICommentRepository
    {
        Task SaveCommentAsync(Comment comment);
        Task<List<Comment>> GetAllCommentsByPostIdANDUserId(Guid postId, Guid userId);
        Task<List<CommentLike>> GetCommentLikesListByUserIdPostId(Guid postId, Guid userId);
        Task SaveCommentLikeAsync(Guid userId, Guid commentId);
        Task DeleteCommentLikeAsync(Guid userId, Guid commentId);
    }
}

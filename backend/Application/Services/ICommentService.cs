using Application.DTOs;


namespace Application.Services
{
    public interface ICommentService
    {
        Task SaveCommentAsync(string textContent, Guid userId, Guid postId);
        Task<List<CommentDTO>> GetAllCommentsByPostIdANDUserId(Guid postId, Guid userId);
        Task SaveCommentLikeAsync(Guid userId, Guid commentId);
        Task DeleteCommentLikeAsync(Guid userId, Guid commentId);
    }
}

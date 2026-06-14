
using Core.Models;
using Application.DTOs;
using Core.Interfaces;

namespace Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _repository;
        
        public CommentService(ICommentRepository repository) { _repository = repository; }


        public async Task SaveCommentAsync(string textContent, Guid userId, Guid postId)
        {
            Comment comment = new Comment(textContent, userId, postId);

            await _repository.SaveCommentAsync(comment);
        }

        public async Task<List<CommentDTO>> GetAllCommentsByPostIdANDUserId(Guid postId, Guid userId)
        {
            List<Comment> comments = await _repository.GetAllCommentsByPostIdANDUserId(postId, userId);
            List<CommentLike> commentsLikes = await _repository.GetCommentLikesListByUserIdPostId(postId, userId);
            List<CommentDTO> commentsDTO = new List<CommentDTO>();

            for (int i = 0; i < comments.Count; i++)
            {
                bool isLiked = false;

                for (int j = 0; j < commentsLikes.Count; j++)
                {
                    if (comments[i].Id == commentsLikes[j].commentId)
                    {
                        isLiked = true;
                        break;
                    }
                }
                commentsDTO.Add(new CommentDTO
                {
                    CommentId = comments[i].Id,
                    AuthorName = comments[i].AuthorName,
                    Content = comments[i].Content,
                    CreatedAt = comments[i].CreatedAt,
                    ReactionCount = comments[i].ReactionCount,
                    UserId = comments[i].UserId,
                    PostId = comments[i].PostId,
                    IsLiked = isLiked
                    });
                
            }
            return commentsDTO;
        }


        public async Task SaveCommentLikeAsync(Guid userId, Guid commentId)
        {
            await _repository.SaveCommentLikeAsync(userId, commentId);
        }
        public async Task DeleteCommentLikeAsync(Guid userId, Guid commentId)
        {
            await _repository.DeleteCommentLikeAsync(userId, commentId);
        }



    }
}

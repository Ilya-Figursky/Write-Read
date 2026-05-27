using System;
using System.Collections.Generic;
using System.Text;
using Core.Models;

namespace Persistence.Repository
{
    public interface IPostRepository
    {
        Task<List<Post>> GetAllPostsAsync();
        Task SavePostAsync(Post post);
        Task SaveCommentAsync(Guid postId, Guid userId, string textContent);
        Task<List<PostLike>> GetLikesListByIdAsync(Guid userId);
        Task SetLikeByPostIdANDUserIdAsync(Guid postId, Guid userId);
        Task RemoveLikeByPostIdANDUserIdAsync(Guid postId, Guid userId);
    }
}

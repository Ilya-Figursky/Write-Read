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
        Task<List<PostLike>> GetPostLikesListByIdAsync(Guid userId);
        Task SetLikeByPostIdANDUserIdAsync(Guid postId, Guid userId);
        Task RemoveLikeByPostIdANDUserIdAsync(Guid postId, Guid userId);
    }
}

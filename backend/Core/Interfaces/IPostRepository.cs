
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
        Task<int> DeletePost(Guid postId);
        Task<List<Post>> GetAllPostByUserIdAsync(Guid userId);
        Task SetComplaint(Complaint complaint);
        Task<List<(Post post, string reason)>> GetAllPostsWithComplaints();
        Task CancelComplaintAsync(Guid postId);
    }
}

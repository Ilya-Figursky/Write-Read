using Application.DTOs;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public interface IPostService
    {
        Task<List<PostDTO>> GetAllPostsAsync();
        Task<List<PostDTO>> GetAllPostsAsync(Guid userId);
        Task SavePostAsync(string textContent, Guid userId);
        Task SetLikeByPostIdANDUserIdAsync(Guid postId, Guid userId);
        Task RemoveLikeByPostIdANDUserIdAsync(Guid postId, Guid userId);
        Task DeletePost(Guid postId);
        Task<List<PostDTO>> GetAllPostByUserIdAsync(Guid userId);
        Task SetComplaint(string reason, Guid userId, Guid postId);
        Task<List<PostWithComplaintDTO>> GetAllPostsWithComplaints();
    }
}

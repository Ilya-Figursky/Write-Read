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
        Task<List<PostDTO>> GetAllPostsByUserIdAsync(Guid userId);
        Task SavePostAsync(string textContent, Guid userId);
        Task SaveCommentAsync(Guid postId, Guid userId, string textContent);
        Task SetLikeByPostIdANDUserIdAsync(Guid postId, Guid userId);
        Task RemoveLikeByPostIdANDUserIdAsync(Guid postId, Guid userId);
    }
}

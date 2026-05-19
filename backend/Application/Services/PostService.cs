using System;
using System.Collections.Generic;
using System.Text;
using Persistence.Repository;
using Core.Models;
using Application.DTOs;
using Core.Interfaces;

namespace Application.Services
{
    public class PostService : IPostService
    {
        IPostRepository _repository;
        IUserReposytory _userRepository;
        
        public async Task<List<PostDTO>> GetAllPosts() 
        { 
            List<Post> posts = new();

            posts = await _repository.GetAllPosts();

            List<PostDTO> postsDTO = new();

            foreach (Post item in posts)
            {
                PostDTO postDTO = new PostDTO();

                postDTO.AuthorName = item.AuthorName;
                postDTO.Comments = item.Comments;
                postDTO.Content = item.Content;
                postDTO.CreatedAt = item.CreatedAt;
                postDTO.ComplaintCount = item.ComplaintCount;
                postDTO.ReactionCount = item.ReactionCount;

                postsDTO.Add(postDTO);
            }
            return postsDTO;
        }

        public async Task SavePost(string textContent, Guid userId)
        {
            User user = await _userRepository.GetUserDataById(userId);

            Post post = new Post(user.Name, textContent, userId);

            await _repository.SavePost(post);
        }



        public PostService(IPostRepository postRepository, IUserReposytory userReposytory) { _repository = postRepository; _userRepository = userReposytory; }
        
    }
}

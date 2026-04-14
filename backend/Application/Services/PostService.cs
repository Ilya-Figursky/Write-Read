using System;
using System.Collections.Generic;
using System.Text;
using Persistence.Repository;
using Core.Models;
using Application.DTOs;

namespace Application.Services
{
    public class PostService : IPostService
    {
        IPostRepository _repository;
        public PostService(IPostRepository postRepository)
        {
            _repository = postRepository;
        }
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
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Persistence.Repository;
using Core.Models;
using Application.DTOs;
using Core.Interfaces;
using Microsoft.VisualBasic.FileIO;

namespace Application.Services
{
    public class PostService : IPostService
    {
        IPostRepository _repository;
        IUserRepository _userRepository;

        public PostService(IPostRepository postRepository, IUserRepository userReposytory) { _repository = postRepository; _userRepository = userReposytory; }

        public async Task<List<PostDTO>> GetAllPostsAsync() 
        { 
            List<Post> posts = new();

            posts = await _repository.GetAllPostsAsync();

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
        public async Task<List<PostDTO>> GetAllPostsAsync(Guid userId)
        {
            List<Post> posts = await _repository.GetAllPostsAsync();

            List<PostLike> likesList = await _repository.GetPostLikesListByIdAsync(userId);

            List<PostDTO> postsDTO = new List<PostDTO>();

            for (int i = 0; i < posts.Count; i++) //sitty algorithm. Remake it. 
            {
                bool isLiked = false;

                for (int j = 0; j < likesList.Count; j++)
                {
                    if (posts[i].Id == likesList[j].postId)
                    {
                        isLiked = true;
                        break;
                    }
                }

                postsDTO.Add(new PostDTO
                {
                    AuthorName = posts[i].AuthorName,
                    Comments = posts[i].Comments,
                    Content = posts[i].Content,
                    CreatedAt = posts[i].CreatedAt,
                    ComplaintCount = posts[i].ComplaintCount,
                    ReactionCount = posts[i].ReactionCount,
                    PostId = posts[i].Id,
                    IsLiked = isLiked
                });
            }


            return postsDTO;
        }
        public async Task SavePostAsync(string textContent, Guid userId)
        {
            User user = await _userRepository.GetUserDataById(userId);

            Post post = new Post(user.Name, textContent, userId);

            await _repository.SavePostAsync(post);
        }

        public async Task SetLikeByPostIdANDUserIdAsync(Guid postId, Guid userId)
        {
            await _repository.SetLikeByPostIdANDUserIdAsync(postId, userId);
        }

        public async Task RemoveLikeByPostIdANDUserIdAsync(Guid postId, Guid userId)
        {
            await _repository.RemoveLikeByPostIdANDUserIdAsync(postId, userId);
        }

        public async Task DeletePost(Guid postId)
        {
            int deletedRows = await _repository.DeletePost(postId);

            Response response;

            if (deletedRows == 1) { response = new Response("Successful"); }
            else
            {
                throw new ArgumentException("Exception in PostService: deletedRows: ", nameof(deletedRows));
            }
        }

        public async Task<List<PostDTO>> GetAllPostByUserIdAsync(Guid userId)
        {
            List<Post> posts = new();

            posts = await _repository.GetAllPostByUserIdAsync(userId);

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

        public async Task SetComplaint(string reason, Guid userId, Guid postId)
        {
            Complaint complaint = new Complaint(reason, userId, postId);

            await _repository.SetComplaint(complaint);
        }

    }
}

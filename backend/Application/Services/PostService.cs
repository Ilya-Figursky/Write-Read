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

        public PostService(IPostRepository postRepository, IUserReposytory userReposytory) { _repository = postRepository; _userRepository = userReposytory; }

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
        public async Task<List<PostDTO>> GetAllPostsByUserIdAsync(Guid userId)
        {
            List<Post> posts = await _repository.GetAllPostsAsync();

            List<Like> likesList = await _repository.GetLikesListByIdAsync(userId);

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
            { /* if (likesList.Count != 0)
                {
                    if (i < likesList.Count)
                    {
                        postDTO.AuthorName = posts[i].AuthorName;
                        postDTO.Comments = posts[i].Comments;
                        postDTO.Content = posts[i].Content;
                        postDTO.CreatedAt = posts[i].CreatedAt;
                        postDTO.ComplaintCount = posts[i].ComplaintCount;
                        postDTO.ReactionCount = posts[i].ReactionCount;
                        postDTO.PostId = posts[i].Id;

                        postDTO.IsLiked = false;

                        postsDTO.Add(postDTO);
                    }
                    else
                    {

                        if (posts[i].Id == likesList[i].postId)
                        {

                            postDTO.AuthorName = posts[i].AuthorName;
                            postDTO.Comments = posts[i].Comments;
                            postDTO.Content = posts[i].Content;
                            postDTO.CreatedAt = posts[i].CreatedAt;
                            postDTO.ComplaintCount = posts[i].ComplaintCount;
                            postDTO.ReactionCount = posts[i].ReactionCount;
                            postDTO.PostId = posts[i].Id;

                            postDTO.IsLiked = true;

                            postsDTO.Add(postDTO);
                        }
                    }
                } 
                else if (likesList.Count == 0)
                {
                    postDTO.AuthorName = posts[i].AuthorName;
                    postDTO.Comments = posts[i].Comments;
                    postDTO.Content = posts[i].Content;
                    postDTO.CreatedAt = posts[i].CreatedAt;
                    postDTO.ComplaintCount = posts[i].ComplaintCount;
                    postDTO.ReactionCount = posts[i].ReactionCount;
                    postDTO.PostId = posts[i].Id;

                    postDTO.IsLiked = false;

                    postsDTO.Add(postDTO);
                }*/
            }
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

        






    }
}

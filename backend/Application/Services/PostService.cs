using System;
using System.Collections.Generic;
using System.Text;
using Persistence.Repository;
using Core.Models;

namespace Application.Services
{
    public class PostService : IPostService
    {
        IPostRepository _repository;
        public PostService(IPostRepository postRepository)
        {
            _repository = postRepository;
        }


        public async Task<List<Post>> GetAllPosts() { return await _repository.GetAllPosts(); }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Core.Models;

namespace Persistence.Repository
{
    public interface IPostRepository
    {
        Task<List<Post>> GetAllPosts();
        //Task PostPost();
    }
}

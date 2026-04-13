using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Repository
{
    public interface IPostRepository
    {
        public async Task GetAllPosts() { }

        public async Task PostPost() { }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Repository
{
    public interface IPostReposytory
    {
        public async Task GetAllPosts() { }

        public async Task PostPost() { }
    }
}

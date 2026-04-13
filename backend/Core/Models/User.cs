using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Core.Models
{
    internal class User
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public List<Post>? UserPosts;

    }
}

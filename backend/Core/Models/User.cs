using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Core.Models
{
    internal class User
    {
        public string Name { get; set; }
        public Guid Id { get; private set; }
        public List<Post> UserPosts { get; private set; }
        public int Password { get; private set; }

        public User(string name, int password)
        {
            Name = name;
            Password = password;
            UserPosts = new();
        }
    }
}

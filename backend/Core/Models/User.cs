using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Core.Models
{
    public class User
    {
        public string Name { get; set; }
        public Guid Id { get; private set; }
        public List<Post> UserPosts { get; private set; }
        public string Password { get; private set; }

        public User(string name, string password)
        {
            Id = Guid.NewGuid();
            Name = name;
            Password = password;
            UserPosts = new();
        }
    }
}

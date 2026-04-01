using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Core.Models
{
    public class Post
    {
        public string AuthorName { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ReactionCount { get; set; }
        public List<Comment>? Comments;

        public Post(string AuthorName, string Content)
        {
            this.AuthorName = AuthorName;
            this.Content = Content;
            CreatedAt = DateTime.Now;
            ReactionCount = 0;
            Comments = null;
        }
    }
}

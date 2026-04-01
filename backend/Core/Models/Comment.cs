using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class Comment
    {
        public string AuthorName { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ReactionCount { get; set; }
        public List<Comment>? Comments;

        public Comment(string AuthorName, string Content)
        {
            this.AuthorName = AuthorName;
            this.Content = Content;
            CreatedAt = DateTime.Now;
            ReactionCount = 0;
            Comments = null;
        }
    }
}

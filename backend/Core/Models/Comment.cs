using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class Comment
    {
        public string AuthorName { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; private set; }
        public int ReactionCount { get; private set; }
        public List<Comment> Comments { get; private set; }
        public int ComplaintCount { get; private set; }

        public Comment(string AuthorName, string Content)
        {
            this.AuthorName = AuthorName;
            this.Content = Content;
            CreatedAt = DateTime.Now;
            ReactionCount = 0;
            ComplaintCount = 0;
            Comments = new();
        }

        public void AddReaction() { ReactionCount++; }
        public void AddComplaint() { ComplaintCount++; }
        public void AddComent(Comment comment) { Comments.Add(comment); }
    }
}

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Core.Models
{
    public class Post
    {
        public Guid Id { get; private set; }
        public string AuthorName { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; private set; }
        public int ReactionCount { get; private set; }
        public List<Comment> Comments { get; private set; }
        public int ComplaintCount { get; private set; }

        public Post(string authorName, string content)
        {
            Id = Guid.NewGuid();
            this.AuthorName = authorName;
            this.Content = content;
            CreatedAt = DateTime.Now;
            ReactionCount = 0;
            ComplaintCount = 0;
            Comments = new();
        }
        public Post() { }

        public void AddReaction() { ReactionCount++; }
        public void AddComplaint() { ComplaintCount++; }
        public void AddComent(Comment comment) { Comments.Add(comment); }
        public void SetId(Guid id) { Id = id; }
        public void SetDateCreatedAt(DateTime time) { CreatedAt = time; }
        public void SetReactionCount(int count) { ReactionCount = count; }
        public void SetComplaintCount(int count) { ComplaintCount = count; }
    }
}

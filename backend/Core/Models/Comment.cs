using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class Comment
    {
        //PostID
        public Guid Id { get; private set; }
        public string AuthorName { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; private set; }
        public int ReactionCount { get; private set; }
        public List<Comment> Comments { get; private set; }
        public int ComplaintCount { get; private set; }
        public Guid UserId { get; private set; }
        public Guid PostId { get; private set; }

        public Comment(string content, Guid userId, Guid postId)
        {
            Id = Guid.NewGuid();
            this.Content = content;
            CreatedAt = DateTime.Now;
            ReactionCount = 0;
            ComplaintCount = 0;
            Comments = new();
            UserId = userId;
            PostId = postId;
        }

        public void AddReaction() { ReactionCount++; }
        public void AddComplaint() { ComplaintCount++; }
        public void AddComent(Comment comment) { Comments.Add(comment); }
        public void SetUserId(Guid id) => UserId = id;
    }
}

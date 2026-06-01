using System;
using System.Collections.Generic;
using System.Data;
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
        public Comment() { }

        public void SetCommentId(Guid id) { Id = id; }
        public void SetCreatedAtTime(DateTime time) { CreatedAt = time; }
        public void SetReactionCount(int count) { ReactionCount = count; }
        public void SetComplaintCount() { ComplaintCount++; }
        public void AddComent(Comment comment) { Comments.Add(comment); }
        public void SetUserId(Guid id) => UserId = id;
        public void SetPostId(Guid id) => PostId = id;
    }
}

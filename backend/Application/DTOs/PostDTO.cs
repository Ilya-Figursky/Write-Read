using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class PostDTO
    {
        public Guid PostId { get; set; }
        public string AuthorName { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ReactionCount { get; set; }
        public List<Comment> Comments { get; set; } // maybe  List<Comment> -> List<CommentDTO>
        public int ComplaintCount { get; set; }
        public bool IsLiked { get; set; }
    }
}



namespace Application.DTOs
{
    public class CommentDTO
    {
        public Guid CommentId { get; set; }
        public string AuthorName { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ReactionCount { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public bool IsLiked { get; set; }
    }
}

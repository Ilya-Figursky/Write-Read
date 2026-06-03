namespace Core.Models
{
    public class Complaint
    {
        public Guid ComplaintId { get; private set; }
        public string Reason { get; set; }
        public Guid UserId { get; private set; }
        public Guid PostId { get; private set; }

        public Complaint(Guid complaintId, string reason, Guid userId, Guid postId) 
        {
            ComplaintId = complaintId;
            Reason = reason;
            UserId = userId;
            PostId = postId;
        }
        public Complaint(string reason, Guid userId, Guid postId)
        {
            ComplaintId = Guid.NewGuid();
            Reason = reason;
            UserId = userId;
            PostId = postId;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class PostWithComplaintDTO
    {
        public Guid PostId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public string Reason { get; set; }
    }
}

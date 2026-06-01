using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class CommentLike
    {
        public Guid userId { get; set; }
        public Guid commentId { get; set; }
    }
}

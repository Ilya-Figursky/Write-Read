using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class PostLike
    {
        public Guid userId { get; set; }
        public Guid postId { get; set; }
    }
}

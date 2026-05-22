using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class Like
    {
        public Guid userId { get; set; }
        public Guid postId { get; set; }
    }
}

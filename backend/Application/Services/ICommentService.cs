using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public interface ICommentService
    {
        Task SaveCommentAsync(string textContent, Guid userId, Guid postId);
    }
}

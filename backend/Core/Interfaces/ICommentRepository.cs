using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    public interface ICommentRepository
    {
        Task SaveCommentAsync(Comment comment);
    }
}

using Application.DTOs;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public interface IPostService
    {
        Task<List<PostDTO>> GetAllPosts();
    }
}

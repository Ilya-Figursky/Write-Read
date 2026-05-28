using System;
using System.Collections.Generic;
using System.Text;
using Persistence.Repository;
using Core.Models;
using Application.DTOs;
using Core.Interfaces;

namespace Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _comRepository;
        
        public CommentService(ICommentRepository repository) { _comRepository = repository; }


        public async Task SaveCommentAsync(string textContent, Guid userId, Guid postId)
        {
            Comment comment = new Comment(textContent, userId, postId);

            await _comRepository.SaveCommentAsync(comment);
        }
    }
}

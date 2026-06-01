using Application.Services;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("wr/home/comments")]
    public class CommentController : Controller
    {
        private readonly ICommentService _comService;

        public CommentController(ICommentService commentService) { _comService = commentService; }

        [HttpPost("saveComment/{userId}/{postId}")]
        public async Task<IActionResult> SaveCommentAsync([FromBody] string textContent, [FromRoute] Guid userId, [FromRoute] Guid postId)
        {
            await _comService.SaveCommentAsync(textContent, userId, postId);

            return Ok();
        }

        [HttpGet("getCommentsByPostIdUserId/{postId}/{userId}")]
        public async Task<IActionResult> GetAllCommentsByPostId([FromRoute] Guid postId, [FromRoute] Guid userId)
        {
            var comments = await _comService.GetAllCommentsByPostIdANDUserId(postId, userId);
            if (comments.Count == 0) { return NotFound(); }
            return Ok(comments);
        }

        [HttpPost("setCommentLike/{userId}/{commentId}")]
        public async Task<IActionResult> SetCommentLikeAsync([FromRoute] Guid userId, [FromRoute] Guid commentId)
        {
            await _comService.SaveCommentLikeAsync(userId, commentId);

            return Ok();
        }

        [HttpDelete("deleteCommentLike/{userId}/{commentId}")]
        public async Task<IActionResult> DeleteCommentLikeAsync([FromRoute] Guid userId, [FromRoute] Guid commentId)
        {
            await _comService.DeleteCommentLikeAsync(userId, commentId);
            return Ok();
        }

    }
}

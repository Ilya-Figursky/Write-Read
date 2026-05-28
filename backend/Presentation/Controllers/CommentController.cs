using Application.Services;
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
    }
}

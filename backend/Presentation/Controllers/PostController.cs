using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Persistence.Repository;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("wr/home")]

    public class PostController : Controller
    {
        IPostService _postService;

        public PostController(IPostService postService) { _postService = postService; }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _postService.GetAllPostsAsync();
            if (posts == null) return NotFound();
            return Ok(posts);
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> SavePost([FromRoute] Guid userId, [FromBody] string textContent)
        {
            await _postService.SavePostAsync(textContent, userId);

            return Ok();
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAllPostsAsync([FromRoute] Guid userId)
        {
            var posts = await _postService.GetAllPostsAsync(userId);

            if (posts == null) return NotFound();

            return Ok(posts);
        }

        [HttpPost("setlike/{postId}/{userId}")]
        public async Task<IActionResult> SetLikeByPostIdANDUserIdAsync([FromRoute] Guid postId, [FromRoute] Guid userId)
        {
            await _postService.SetLikeByPostIdANDUserIdAsync(postId, userId);

            return Ok();
        }

        [HttpPost("remuvelike/{postId}/{userId}")]
        public async Task<IActionResult> RemoveLikeByPostIdANDUserIdAsync([FromRoute] Guid postId, [FromRoute] Guid userId)
        {
            await _postService.RemoveLikeByPostIdANDUserIdAsync(postId, userId);

            return Ok();
        }

        [HttpPost("deletePost/{postId}")]
        public async Task<IActionResult> DeletePost([FromRoute]Guid postId)
        {
            await _postService.DeletePost(postId);
            return Ok();
        }

        [HttpGet("usersposts/{userId}")]
        public async Task<IActionResult> GetAllPostByUserIdAsync([FromRoute] Guid userId)
        {
            var posts = await _postService.GetAllPostByUserIdAsync(userId);
            
            return Ok(posts);
        }

        [HttpPost("complaint/{userId}/{postId}")]
        public async Task<IActionResult> SetComplaint([FromBody]string reason, [FromRoute] Guid userId, [FromRoute] Guid postId)
        {
            await _postService.SetComplaint(reason, userId, postId);
            return Ok();
        }

        
    }
}

using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repository;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("wr/home")]

    public class PostController : Controller
    {
        IPostService _postService;

        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _postService.GetAllPosts();
            if (posts == null) return NotFound();
            return Ok(posts);

            // add register all new serviers before start test

        }
        [HttpPost("{userId}")]
        public async Task<IActionResult> SavePost([FromRoute] Guid userId, [FromBody] string textContent)
        {
            await _postService.SavePost(textContent, userId);

            return Ok();
        }

        public PostController(IPostService postService){ _postService = postService; }
    }
}

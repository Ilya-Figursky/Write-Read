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

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public IActionResult GetAllNotes()
        {

        }

    }
}

using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repository;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("wr/register")]

    public class UserController : Controller
    {
        IUserService _userService;

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] UserRegisterDTO userRegister)
        {
            var result = await _userService.SignUp(userRegister);

            return Ok(result);
        }
        

        public UserController(IUserService userService) { _userService = userService; }
    }
}

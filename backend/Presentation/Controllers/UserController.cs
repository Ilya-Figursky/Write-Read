using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repository;
using Core.Models;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("wr/register")]

    public class UserController : Controller
    {
        IUserService _userService;

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] UserRegisterDTO userRegister)
        {
            var result = await _userService.SignUp(userRegister);

            return Ok(result);
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] UserRegisterDTO userRegister)
        {
            string name = userRegister.Name;
            string password = userRegister.Password;

            (User userData, int statusCode) = await _userService.SignIn(name, password);

            if (userData == null & statusCode == 404) 
            { 
                return NotFound(); 
            }
            else if(userData == null & statusCode == 401)
            {
                return Unauthorized();
            }
            else
            {
                return Ok(userData);
            }
        }
        

        public UserController(IUserService userService) { _userService = userService; }
    }
}

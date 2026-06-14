using Application.DTOs;
using Core.Models;

namespace Application.Services
{
    public interface IUserService
    {
        Task<User> SignUp(UserRegisterDTO userRegister);
        Task<(User user, int statusCode)> SignIn(string name, string password);
        //Task<User> GetUserLoginStatus();
    }
}

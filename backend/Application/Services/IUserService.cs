using Application.DTOs;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public interface IUserService
    {
        Task<User> SignUp(UserRegisterDTO userRegister);
        //Task<User> SignIn();
        //Task<User> GetUserLoginStatus();
    }
}

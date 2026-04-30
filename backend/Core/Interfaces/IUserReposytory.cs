using System;
using System.Collections.Generic;
using System.Text;
using Core.Models;

namespace Core.Interfaces
{
    public interface IUserReposytory
    {
        Task<User> SignUp(User user);
        //Task<User> SignIn();
        //Task<User> GetUserLoginStatus();
    }
}

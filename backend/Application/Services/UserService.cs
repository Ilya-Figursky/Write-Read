using Application.DTOs;
using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public class UserService: IUserService
    {
        IUserReposytory _repository;

        public UserService( IUserReposytory reposytory) { _repository = reposytory; }

        public async Task<User> SignUp(UserRegisterDTO userRegister)
        {
            string name = userRegister.Name;
            string password = userRegister.Password;

            User user = new User(name, password);

            return await _repository.SignUp(user);
        }
    }
}

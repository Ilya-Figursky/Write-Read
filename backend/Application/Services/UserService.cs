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

        public async Task<(User user, int statusCode)> SignIn(string name, string password)
        {
            User user = new User(name);

            User userData = await _repository.SignIn(user);

            if (userData.Id == Guid.Empty && userData.Password == "") //chek data from Db, no frontend
            { 
                return (null, 404); 
            } else if(userData.Password != password) //incorrect password
            {
                return (null, 401);//add alert in frontend about incorrect password
            }
            else
            {
                return (userData, 200);
            }
        }
    }
}



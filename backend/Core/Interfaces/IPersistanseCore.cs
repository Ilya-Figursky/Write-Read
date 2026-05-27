using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    public interface IPersistanseCore
    {
        Task<User> GetUserDataById(Guid id);
    }
}

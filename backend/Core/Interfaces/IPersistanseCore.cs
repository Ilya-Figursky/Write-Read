using Core.Models;


namespace Core.Interfaces
{
    public interface IPersistanseCore
    {
        Task<User> GetUserDataById(Guid id);
    }
}

using Core.Interfaces;
using Entities.DTOs;

namespace Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
    }
}

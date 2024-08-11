using Application.DTOs;
using Application.Interfaces;

namespace Application.Services.Implementations
{
    public class UserService : IUserService
    {
        public Task<UserDto> GetUserByIdAsync(string userId)
        {
            // Implementation logic here
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            // Implementation logic here
            throw new NotImplementedException();
        }

        public Task AddUserAsync(UserDto user)
        {
            // Implementation logic here
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(UserDto user)
        {
            // Implementation logic here
            throw new NotImplementedException();
        }

        public Task DeleteUserAsync(string userId)
        {
            // Implementation logic here
            throw new NotImplementedException();
        }
    }
}

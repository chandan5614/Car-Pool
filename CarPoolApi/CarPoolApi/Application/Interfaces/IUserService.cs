using Application.DTOs;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(string userId);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task AddUserAsync(UserDto user);
        Task UpdateUserAsync(UserDto user);
        Task DeleteUserAsync(string userId);
        Task<UserDto> AuthenticateAsync(string email, string password);
        Task<UserDto> RegisterAsync(string email, string password);
    }
}

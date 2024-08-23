using Application.DTOs;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(Guid userId);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task AddUserAsync(UserDto userDto);
        Task UpdateUserAsync(UserDto userDto);
        Task DeleteUserAsync(Guid userId);
        Task<UserDto> AuthenticateAsync(string email, string password);
        Task<UserDto> RegisterAsync(string email, string password);
    }
}

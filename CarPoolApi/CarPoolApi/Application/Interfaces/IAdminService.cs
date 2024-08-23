using Application.DTOs;

namespace Application.Interfaces
{
    public interface IAdminService
    {
        Task<UserDto> GetUserByIdAsync(Guid userId);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<IEnumerable<UserDto>> GetUsersByRoleAsync(string role);
        Task AddUserAsync(UserDto userDto);
        Task UpdateUserAsync(UserDto userDto);
        Task DeleteUserAsync(Guid userId);
        Task<UserDto> RegisterAdminAsync(string email, string password);
        Task<UserDto> AuthenticateAdminAsync(string email, string password);
    }
}

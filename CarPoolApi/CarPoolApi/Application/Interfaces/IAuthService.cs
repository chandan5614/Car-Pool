using System.Threading.Tasks;
using Application.DTOs;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto> AuthenticateUserAsync(string email, string password);
        Task<string> GenerateTokenAsync(UserDto user);
        Task<bool> ValidateTokenAsync(string token);
        Task<UserDto> GetUserByTokenAsync(string token);
    }
}

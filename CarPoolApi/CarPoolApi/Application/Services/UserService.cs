using Application.DTOs;
using Application.Interfaces;
using Core.Interfaces;
using Entities.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserDto> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return null;

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return passwordVerificationResult == PasswordVerificationResult.Success ? MapToDto(user) : null;
        }

        public async Task<UserDto> RegisterAsync(string email, string password)
        {
            var user = new User { Email = email };
            user.PasswordHash = _passwordHasher.HashPassword(user, password);
            await _userRepository.AddAsync(user);
            return MapToDto(user);
        }

        public async Task<UserDto> GetUserByIdAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(Guid.Parse(userId));
            return user == null ? null : MapToDto(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(MapToDto);
        }

        public async Task AddUserAsync(UserDto userDto)
        {
            var user = MapToEntity(userDto);
            user.PasswordHash = _passwordHasher.HashPassword(user, userDto.Password);
            await _userRepository.AddAsync(user);
        }

        public async Task UpdateUserAsync(UserDto userDto)
        {
            var user = MapToEntity(userDto);
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(string userId)
        {
            await _userRepository.DeleteAsync(Guid.Parse(userId));
        }

        private UserDto MapToDto(User user)
        {
            return new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString(),
                VehicleDetails = user.VehicleDetails != null ? new VehicleDetailsDto
                {
                    Make = user.VehicleDetails.Make,
                    Model = user.VehicleDetails.Model,
                    Year = user.VehicleDetails.Year,
                    LicensePlate = user.VehicleDetails.LicensePlate
                } : null,
                Ratings = user.Ratings?.Select(r => new RatingDto
                {
                    RatingId = r.RatingId,
                    RideId = r.RideId,
                    UserId = user.UserId,
                    Rating = r.RatingValue, 
                    Comments = r.Comments,
                    Timestamp = r.Timestamp
                }).ToList() ?? new List<RatingDto>()
            };
        }

        private User MapToEntity(UserDto userDto)
        {
            return new User
            {
                UserId = userDto.UserId,
                Name = userDto.Name,
                Email = userDto.Email,
                Role = Enum.Parse<UserRole>(userDto.Role),
                VehicleDetails = userDto.VehicleDetails != null ? new Vehicle
                {
                    Make = userDto.VehicleDetails.Make,
                    Model = userDto.VehicleDetails.Model,
                    Year = userDto.VehicleDetails.Year,
                    LicensePlate = userDto.VehicleDetails.LicensePlate
                } : null,
                Ratings = userDto.Ratings?.Select(r => new Rating
                {
                    RatingId = r.RatingId,
                    RideId = r.RideId,
                    UserId = r.UserId,
                    RatingValue = r.Rating,
                    Comments = r.Comments,
                    Timestamp = r.Timestamp
                }).ToList() ?? new List<Rating>()
            };
        }
    }
}

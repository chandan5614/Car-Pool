using Application.DTOs;
using Application.Interfaces;
using Core.Interfaces;
using Entities.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Core.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AdminService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserDto> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user == null ? null : MapToDto(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(MapToDto);
        }

        public async Task<IEnumerable<UserDto>> GetUsersByRoleAsync(string role)
        {
            var users = await _userRepository.GetUsersByRoleAsync(role);
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

        public async Task DeleteUserAsync(Guid userId)
        {
            await _userRepository.DeleteAsync(userId);
        }

        public async Task<UserDto> RegisterAdminAsync(string email, string password)
        {
            var user = new User { Email = email, Role = UserRole.Admin };
            user.PasswordHash = _passwordHasher.HashPassword(user, password);
            await _userRepository.AddAsync(user);
            return MapToDto(user);
        }

        public async Task<UserDto> AuthenticateAdminAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || user.Role != UserRole.Admin) return null;

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return passwordVerificationResult == PasswordVerificationResult.Success ? MapToDto(user) : null;
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
                    UserId = r.UserId,
                    Rating = r.RatingValue,
                    Comments = r.Comments,
                    Timestamp = r.Timestamp
                }).ToList() ?? new List<RatingDto>(),
                VerificationDocuments = user.VerificationDocuments?.Select(d => new VerificationDocumentDto
                {
                    DocumentType = d.DocumentType,
                    DocumentUrl = d.DocumentUrl,
                    UploadDate = d.UploadDate // Ensure this is nullable if your model allows it
                }).ToList() ?? new List<VerificationDocumentDto>()
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

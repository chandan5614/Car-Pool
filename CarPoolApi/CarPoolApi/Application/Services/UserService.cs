using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;
using Core.Interfaces;
using Entities.DTOs;

namespace Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
                Role = user.Role.ToString(), // Assuming UserRole is an enum
                VehicleDetails = new VehicleDetailsDto
                {
                    Make = user.VehicleDetails.Make,
                    Model = user.VehicleDetails.Model,
                    Year = user.VehicleDetails.Year,
                    LicensePlate = user.VehicleDetails.LicensePlate
                },
                Ratings = user.Ratings.Select(r => new RatingDto
                {
                    // Map Rating to RatingDto
                    // Assuming RatingDto has similar properties
                    // Example:
                    // RatingId = r.RatingId,
                    // Score = r.Score,
                    // Comment = r.Comment
                }).ToList()
            };
        }

        private User MapToEntity(UserDto userDto)
        {
            return new User
            {
                UserId = userDto.UserId,
                Name = userDto.Name,
                Email = userDto.Email,
                Role = Enum.Parse<UserRole>(userDto.Role), // Assuming UserRole is an enum
                VehicleDetails = new Vehicle
                {
                    Make = userDto.VehicleDetails.Make,
                    Model = userDto.VehicleDetails.Model,
                    Year = userDto.VehicleDetails.Year,
                    LicensePlate = userDto.VehicleDetails.LicensePlate
                },
                Ratings = userDto.Ratings.Select(r => new Rating
                {
                    // Map RatingDto to Rating
                    // Assuming Rating has similar properties
                    // Example:
                    // RatingId = r.RatingId,
                    // Score = r.Score,
                    // Comment = r.Comment
                }).ToList()
            };
        }
    }
}

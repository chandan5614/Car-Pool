using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _adminService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                // Log exception here
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            try
            {
                var user = await _adminService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                // Log exception here
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("users/role/{role}")]
        public async Task<IActionResult> GetUsersByRole(string role)
        {
            try
            {
                var users = await _adminService.GetUsersByRoleAsync(role);
                return Ok(users);
            }
            catch (Exception ex)
            {
                // Log exception here
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("users")]
        public async Task<IActionResult> AddUser([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest("User data is null");
            }

            try
            {
                await _adminService.AddUserAsync(userDto);
                return CreatedAtAction(nameof(GetUserById), new { userId = userDto.UserId }, userDto);
            }
            catch (Exception ex)
            {
                // Log exception here
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("users")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest("User data is null");
            }

            try
            {
                await _adminService.UpdateUserAsync(userDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log exception here
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("users/{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            try
            {
                await _adminService.DeleteUserAsync(userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log exception here
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminDto registerAdminDto)
        {
            if (registerAdminDto == null)
            {
                return BadRequest("Registration data is null");
            }

            try
            {
                var admin = await _adminService.RegisterAdminAsync(registerAdminDto.Email, registerAdminDto.Password);
                return CreatedAtAction(nameof(GetUserById), new { userId = admin.UserId }, admin);
            }
            catch (Exception ex)
            {
                // Log exception here
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAdmin([FromBody] AuthenticateAdminDto authenticateAdminDto)
        {
            if (authenticateAdminDto == null)
            {
                return BadRequest("Authentication data is null");
            }

            try
            {
                var admin = await _adminService.AuthenticateAdminAsync(authenticateAdminDto.Email, authenticateAdminDto.Password);
                if (admin == null)
                {
                    return Unauthorized();
                }
                return Ok(admin);
            }
            catch (Exception ex)
            {
                // Log exception here
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

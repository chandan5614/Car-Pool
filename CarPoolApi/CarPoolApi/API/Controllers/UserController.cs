using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;
using System.Threading.Tasks;
using CarPoolApi.API.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterModel model)
        {
            var userDto = new UserDto
            {
                Name = model.Name,
                Email = model.Email,
                // Hash password or other logic
            };
            await _userService.AddUserAsync(userDto);
            return CreatedAtAction(nameof(GetUserById), new { id = userDto.UserId }, userDto);
        }
    }
}

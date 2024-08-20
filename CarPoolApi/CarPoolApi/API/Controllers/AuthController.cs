using Application.DTOs;
using Application.Interfaces;
using CarPoolApi.API.Models;
using CarPoolApi.Utils.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CarPoolApi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userService.AuthenticateAsync(model.Email, model.Password);
            if (user == null) return Unauthorized();

            var secretKey = _configuration["Jwt:Secret"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var token = JwtTokenHelper.GenerateToken(user, secretKey, issuer, audience);
            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userDto = new UserDto
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password
            };

            var user = await _userService.RegisterAsync(model.Email, model.Password);
            return CreatedAtAction(nameof(Register), new { id = user.UserId }, user);
        }
    }
}

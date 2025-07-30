using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs.User;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace RealEstate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            try
            {
                var (token, user) = await _userService.RegisterAsync(dto);
                return Created("", new { token, user });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            try
            {
                var (token, user) = await _userService.LoginAsync(dto);
                return Created("", new { token, user });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetProfile()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
                return Unauthorized("Недействительный токен.");

            var user = await _userService.GetProfileAsync(userId);
            return user == null ? NotFound() : Ok(user);
        }
    }
}

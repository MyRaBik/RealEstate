using Application.DTOs.User;
using Application.Interfaces;
using Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public UserService(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        public async Task<(string Token, UserDto User)> RegisterAsync(UserRegisterDto dto)
        {
            if (dto.Password.Length < 6 || dto.Password.Length > 12)
                throw new ArgumentException("Пароль должен быть от 6 до 12 символов");

            var existing = await _userRepository.GetByMailAsync(dto.Mail.ToLower());
            if (existing != null)
                throw new InvalidOperationException("Пользователь с такой почтой уже зарегистрирован");

            var user = new User
            {
                Mail = dto.Mail,
                Password = dto.Password,
                Role = "user",
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return (GenerateToken(user), ToDto(user));
        }

        public async Task<(string Token, UserDto User)> LoginAsync(UserLoginDto dto)
        {
            var user = await _userRepository.GetByMailAsync(dto.Mail);
            if (user == null || user.Password != dto.Password)
                throw new UnauthorizedAccessException("Неверная почта или пароль");

            return (GenerateToken(user), ToDto(user));
        }

        public async Task<UserDto?> GetProfileAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user == null ? null : ToDto(user);
        }

        private static UserDto ToDto(User user) => new()
        {
            UserId = user.UserId,
            Mail = user.Mail,
            Role = user.Role
        };

        private string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Mail),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

using Application.DTOs.User;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<(string Token, UserDto User)> RegisterAsync(UserRegisterDto dto);
        Task<(string Token, UserDto User)> LoginAsync(UserLoginDto dto);
        Task<UserDto?> GetProfileAsync(int userId);
    }
}

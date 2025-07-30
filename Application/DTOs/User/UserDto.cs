namespace Application.DTOs.User
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Mail { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}

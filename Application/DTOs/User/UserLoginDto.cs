using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public class UserLoginDto
    {
        [Required]
        public string Mail { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}

using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "Поле Email обязательно")]
        [EmailAddress(ErrorMessage = "Введите корректный Email")]
        public string Mail { get; set; } = null!;

        [Required(ErrorMessage = "Поле Пароль обязательно")]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "Пароль должен быть от 6 до 12 символов")]
        public string Password { get; set; } = null!;
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class User
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("mail")]
        [Required]
        public string Mail { get; set; } = null!;

        [Column("password")]
        [Required]
        public string Password { get; set; } = null!;

        [Column("role")]
        [Required]
        public string Role { get; set; } = null!; // "user", "admin"

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public ICollection<ChatSession> ChatSessions { get; set; } = new List<ChatSession>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();

    }
}

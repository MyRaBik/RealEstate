using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class ChatSession
    {
        [Key]
        [Column("chat_session_id")]
        public int ChatSessionId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Column("started_at")]
        public DateTime StartedAt { get; set; }

        [Column("is_closed")]
        public bool IsClosed { get; set; }

        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}

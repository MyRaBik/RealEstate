using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Message
    {
        [Key]
        [Column("message_id")]
        public int MessageId { get; set; }

        [Column("chat_session_id")]
        public int ChatSessionId { get; set; }

        [ForeignKey("ChatSessionId")]
        public ChatSession ChatSession { get; set; } = null!;

        [Column("sender_id")]
        public int SenderId { get; set; }

        [ForeignKey("SenderId")]
        public User Sender { get; set; } = null!;

        [Column("sender_type")]
        [Required]
        public string SenderType { get; set; } = null!; // user, admin, bot

        [Column("text")]
        [Required]
        public string Text { get; set; } = null!;

        [Column("sent_at")]
        public DateTime SentAt { get; set; }
    }
}

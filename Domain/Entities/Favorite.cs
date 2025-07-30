using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Favorite
    {
        [Key]
        [Column("favorites_id")]
        public int FavoritesId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Column("document_id")]
        public int DocumentId { get; set; }

        [ForeignKey("DocumentId")]
        public Document Document { get; set; } = null!;
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Document
    {
        [Key]
        [Column("document_id")]
        public int DocumentId { get; set; }

        [Column("document_type")]
        [Required]
        public string DocumentType { get; set; } = null!; // template, article, case_law

        [Column("topic")]
        [Required]
        public string Topic { get; set; } = null!;

        [Column("title")]
        [Required]
        public string Title { get; set; } = null!;

        [Column("description")]
        [Required]
        public string Description { get; set; } = null!;

        [Column("image_url")]
        public string? ImageUrl { get; set; }

        [Column("text")]
        [Required]
        public string Text { get; set; } = null!;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "jsonb")]
        public List<string> Tags { get; set; } = new();
        public TemplateFile? TemplateFile { get; set; } // null для не шаблонов
    }
}

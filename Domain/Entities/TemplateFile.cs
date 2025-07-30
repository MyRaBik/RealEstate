using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class TemplateFile
    {
        [Key]
        [Column("template_file_id")]
        public int TemplateFileId { get; set; }

        [Column("document_id")]
        public int DocumentId { get; set; }

        [ForeignKey("DocumentId")]
        public Document Document { get; set; } = null!;

        [Column("mime_type")]
        public string MimeType { get; set; } = null!;

        [Column("file_path")]
        public string FilePath { get; set; } = null!;
    }
}

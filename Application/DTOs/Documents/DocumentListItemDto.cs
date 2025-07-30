namespace Application.DTOs.Documents
{
    public class DocumentListItemDto
    {
        public int DocumentId { get; set; }
        public string DocumentType { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Topic { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}

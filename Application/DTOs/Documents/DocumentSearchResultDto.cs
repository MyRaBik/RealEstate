namespace Application.DTOs.Documents
{
    public class DocumentSearchResultDto
    {
        public int DocumentId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string Topic { get; set; } = string.Empty;
        public List<string> MatchedTags { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }
}

namespace Application.DTOs.Documents
{
    public class DocumentTopicGroupDto
    {
        public List<DocumentShortDto> Article { get; set; } = new();
        public List<DocumentShortDto> CaseLaw { get; set; } = new();
        public List<DocumentShortDto> Templates { get; set; } = new();
    }
}

using Application.DTOs.Documents;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly ITemplateFileRepository _templateFileRepository;
        public DocumentService(IDocumentRepository documentRepository, ITemplateFileRepository templateFileRepository)
        {
            _documentRepository = documentRepository;
            _templateFileRepository = templateFileRepository;
        }

        public async Task<IEnumerable<DocumentListItemDto>> GetAllDocumentsAsync()
        {
            Console.WriteLine("[DocumentService][GetAllDocumentsAsync] Старт");

            var documents = await _documentRepository.GetAllAsync();

            Console.WriteLine($"[DocumentService][GetAllDocumentsAsync] Получено из БД: {documents.Count()} записей");

            return documents.Select(d => new DocumentListItemDto
            {
                DocumentId = d.DocumentId,
                DocumentType = d.DocumentType,
                Topic = d.Topic,
                Title = d.Title,
                Description = d.Description,
                CreatedAt = d.CreatedAt
            });
        }


        public async Task<DocumentFullDto?> GetDocumentByIdAsync(int id)
        {
            var doc = await _documentRepository.GetByIdAsync(id);

            if (doc == null)
                return null;

            return new DocumentFullDto
            {
                DocumentId = doc.DocumentId,
                DocumentType = doc.DocumentType,
                Topic = doc.Topic,
                Title = doc.Title,
                Description = doc.Description,
                Text = doc.Text,
                ImageUrl = doc.ImageUrl,
                CreatedAt = doc.CreatedAt
            };
        }

        public async Task<DocumentTopicGroupDto?> GetDocumentsByTopicAsync(string topic)
        {
            if (string.IsNullOrWhiteSpace(topic))
                throw new ArgumentException("Тема не может быть пустой");

            var all = await _documentRepository.GetByTopicAsync(topic);

            if (!all.Any())
                return null;

            return new DocumentTopicGroupDto
            {
                Article = all
                    .Where(d => d.DocumentType == "article")
                    .Select(ToShortDto).ToList(),

                CaseLaw = all
                    .Where(d => d.DocumentType == "case_law")
                    .Select(ToShortDto).ToList(),

                Templates = all
                    .Where(d => d.DocumentType == "template")
                    .Select(ToShortDto).ToList()
            };
        }

        private static DocumentShortDto ToShortDto(Document d) => new()
        {
            DocumentId = d.DocumentId,
            Title = d.Title,
            Description = d.Description
        };

        public async Task<IEnumerable<DocumentSearchResultDto>> SearchDocumentsAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query) || query.Trim().Length < 2)
                throw new ArgumentException("Поисковый запрос должен быть не короче 2 символов");

            var results = await _documentRepository.SearchFullTextAsync(query);

            var loweredWords = query.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return results.Select(d => new DocumentSearchResultDto
            {
                DocumentId = d.DocumentId,
                Title = d.Title,
                Description = d.Description,
                DocumentType = d.DocumentType,
                Topic = d.Topic,
                CreatedAt = d.CreatedAt,
                MatchedTags = d.Tags
                    .Where(tag => loweredWords.Any(word => tag.ToLower().Contains(word)))
                    .ToList()
            });
        }

        public async Task<(TemplateFile file, string filePath)?> GetTemplateFileAsync(int documentId)
        {
            var doc = await _documentRepository.GetByIdAsync(documentId);
            if (doc == null)
            {
                Console.WriteLine("[DocumentService] Документ не найден.");
                return null;
            }

            if (doc.DocumentType != "template")
            {
                Console.WriteLine($"[DocumentService] Документ {documentId} не является шаблоном: {doc.DocumentType}");
                return null;
            }

            var file = await _templateFileRepository.GetByDocumentIdAsync(documentId);
            if (file == null)
            {
                Console.WriteLine("[DocumentService] TemplateFile не найден в БД.");
                return null;
            }

            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.FilePath);
            Console.WriteLine($"[DocumentService] Проверка файла по пути: {fullPath}");

            if (!System.IO.File.Exists(fullPath))
            {
                Console.WriteLine("[DocumentService] Физический файл отсутствует.");
                return null;
            }

            Console.WriteLine("[DocumentService] Файл найден. Готов к отправке.");
            return (file, fullPath);
        }

    }
}

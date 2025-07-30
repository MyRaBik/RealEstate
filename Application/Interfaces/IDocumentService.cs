using Application.DTOs.Documents;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IDocumentService
    {
        Task<IEnumerable<DocumentListItemDto>> GetAllDocumentsAsync();
        Task<DocumentFullDto?> GetDocumentByIdAsync(int id);
        Task<DocumentTopicGroupDto?> GetDocumentsByTopicAsync(string topic);
        Task<IEnumerable<DocumentSearchResultDto>> SearchDocumentsAsync(string query);
        Task<(TemplateFile file, string filePath)?> GetTemplateFileAsync(int documentId);
    }
}

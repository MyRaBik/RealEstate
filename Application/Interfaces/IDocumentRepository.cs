using Domain.Entities;

namespace Application.Interfaces
{
    public interface IDocumentRepository
    {
        Task<IEnumerable<Document>> GetAllAsync();
        Task<Document?> GetByIdAsync(int id);
        Task<List<Document>> GetByTopicAsync(string topic);
        Task<List<Document>> SearchFullTextAsync(string query);
    }
}

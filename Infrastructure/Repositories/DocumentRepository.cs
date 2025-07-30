using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using DbLib;
using NpgsqlTypes;

namespace Infrastructure.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly AppDbContext _context;

        public DocumentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Document>> GetAllAsync()
        {
            Console.WriteLine("[DocumentRepository][GetAllAsync] Чтение из БД");
            var result = await _context.Documents.ToListAsync();
            Console.WriteLine($"[DocumentRepository][GetAllAsync] Прочитано: {result.Count}");
            return result;
        }


        public async Task<Document?> GetByIdAsync(int id)
        {
            return await _context.Documents.FindAsync(id);
        }

        public async Task<List<Document>> GetByTopicAsync(string topic)
        {
            return await _context.Documents
                .Where(d => d.Topic == topic)
                .ToListAsync();
        }

        public async Task<List<Document>> SearchFullTextAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<Document>();

            var words = query.Trim().ToLowerInvariant().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var tsQuery = string.Join(" | ", words);

            var sql = @"
                SELECT *
                FROM ""Documents""
                WHERE to_tsvector('russian', title || ' ' || description || ' ' || text)
                      @@ to_tsquery('russian', {0})
                ORDER BY ts_rank(to_tsvector('russian', title || ' ' || description || ' ' || text), to_tsquery('russian', {0})) DESC
            ";

            return await _context.Documents.FromSqlRaw(sql, tsQuery).ToListAsync();
        }
    }
}

using Application.Interfaces;
using DbLib;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TemplateFileRepository: ITemplateFileRepository
    {
        private readonly AppDbContext _context;

        public TemplateFileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TemplateFile?> GetByDocumentIdAsync(int documentId)
        {
            return await _context.TemplateFiles
                .FirstOrDefaultAsync(f => f.DocumentId == documentId);
        }
    }
}

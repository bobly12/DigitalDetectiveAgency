using DigitalDetectiveAgency.Data;
using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalDetectiveAgency.Repositories.Implementations
{
    public class CaseRepository : ICaseRepository
    {
        private readonly ApplicationDbContext _context;

        public CaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Added back standard GetByIdAsync to support your CaseService accusation lookup
        public async Task<Case?> GetByIdAsync(int id)
        {
            return await _context.Cases.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Case?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Cases
                .Include(c => c.Suspects)
                .Include(c => c.Evidences)
                .Include(c => c.Witnesses)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Case>> GetActiveCasesAsync()
        {
            return await _context.Cases.Where(c => c.IsActive).ToListAsync();
        }
    }
}
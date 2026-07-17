using Microsoft.EntityFrameworkCore;
using DigitalDetectiveAgency.Data;
using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Repositories.Interfaces;

namespace DigitalDetectiveAgency.Repositories.Implementations
{
    public class SuspectRepository : ISuspectRepository
    {
        private readonly ApplicationDbContext _context;

        public SuspectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Suspect>> GetByCaseIdAsync(int caseId)
        {
            return await _context.Suspects
                .Where(s => s.CaseId == caseId)
                .ToListAsync();
        }

        public async Task<Suspect?> GetByIdAsync(int id)
        {
            return await _context.Suspects.FindAsync(id);
        }
    }
}
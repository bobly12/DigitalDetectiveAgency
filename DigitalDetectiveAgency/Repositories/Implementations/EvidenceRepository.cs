using Microsoft.EntityFrameworkCore;
using DigitalDetectiveAgency.Data;
using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Repositories.Interfaces;

namespace DigitalDetectiveAgency.Repositories.Implementations
{
    public class EvidenceRepository : IEvidenceRepository
    {
        private readonly ApplicationDbContext _context;

        public EvidenceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Evidence>> GetByCaseIdAsync(int caseId)
        {
            return await _context.Evidences
                .Where(e => e.CaseId == caseId)
                .ToListAsync();
        }
    }
}
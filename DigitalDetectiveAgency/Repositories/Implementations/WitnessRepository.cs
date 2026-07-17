using Microsoft.EntityFrameworkCore;
using DigitalDetectiveAgency.Data;
using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Repositories.Interfaces;

namespace DigitalDetectiveAgency.Repositories.Implementations
{
    public class WitnessRepository : IWitnessRepository
    {
        private readonly ApplicationDbContext _context;

        public WitnessRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Witness>> GetByCaseIdAsync(int caseId)
        {
            return await _context.Witnesses
                .Where(w => w.CaseId == caseId)
                .ToListAsync();
        }
    }
}
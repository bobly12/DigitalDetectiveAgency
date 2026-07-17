using Microsoft.EntityFrameworkCore;
using DigitalDetectiveAgency.Data;
using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Repositories.Interfaces;

namespace DigitalDetectiveAgency.Repositories.Implementations
{
    public class CaseAttemptRepository : ICaseAttemptRepository
    {
        private readonly ApplicationDbContext _context;

        public CaseAttemptRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CaseAttempt?> GetExistingAttemptAsync(string userId, int caseId)
        {
            return await _context.CaseAttempts
                .FirstOrDefaultAsync(ca => ca.UserId == userId && ca.CaseId == caseId);
        }

        public async Task<List<CaseAttempt>> GetByUserIdAsync(string userId)
        {
            return await _context.CaseAttempts
                .Include(ca => ca.Case)
                .Include(ca => ca.AccusedSuspect)
                .Where(ca => ca.UserId == userId)
                .OrderByDescending(ca => ca.AttemptedAt)
                .ToListAsync();
        }

        public async Task AddAsync(CaseAttempt attempt)
        {
            await _context.CaseAttempts.AddAsync(attempt);
            await _context.SaveChangesAsync();
        }
    }
}
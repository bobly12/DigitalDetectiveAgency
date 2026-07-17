using DigitalDetectiveAgency.Models.Entities;

namespace DigitalDetectiveAgency.Repositories.Interfaces
{
    public interface ICaseAttemptRepository
    {
        Task<CaseAttempt?> GetExistingAttemptAsync(string userId, int caseId);
        Task<List<CaseAttempt>> GetByUserIdAsync(string userId);
        Task AddAsync(CaseAttempt attempt);
    }
}
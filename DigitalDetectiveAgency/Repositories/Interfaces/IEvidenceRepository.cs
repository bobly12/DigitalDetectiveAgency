using DigitalDetectiveAgency.Models.Entities;

namespace DigitalDetectiveAgency.Repositories.Interfaces
{
    public interface IEvidenceRepository
    {
        Task<List<Evidence>> GetByCaseIdAsync(int caseId);
    }
}
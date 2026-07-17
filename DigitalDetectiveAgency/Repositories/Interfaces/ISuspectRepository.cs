using DigitalDetectiveAgency.Models.Entities;

namespace DigitalDetectiveAgency.Repositories.Interfaces
{
    public interface ISuspectRepository
    {
        Task<List<Suspect>> GetByCaseIdAsync(int caseId);
        Task<Suspect?> GetByIdAsync(int id);
    }
}
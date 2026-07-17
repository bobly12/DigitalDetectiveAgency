using DigitalDetectiveAgency.Models.Entities;

namespace DigitalDetectiveAgency.Repositories.Interfaces
{
    public interface IWitnessRepository
    {
        Task<List<Witness>> GetByCaseIdAsync(int caseId);
    }
}
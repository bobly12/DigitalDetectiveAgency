using DigitalDetectiveAgency.Models.Entities;

namespace DigitalDetectiveAgency.Repositories.Interfaces
{
    public interface ICaseRepository
    {
        Task<Case?> GetByIdAsync(int id);
        Task<Case?> GetByIdWithDetailsAsync(int id); // Loads Evidence, Witnesses, and Suspects together
        Task<List<Case>> GetActiveCasesAsync();
    }
}
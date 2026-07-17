using DigitalDetectiveAgency.Models.DTOs.Case;
using DigitalDetectiveAgency.Models.DTOs.CaseAttempt;

namespace DigitalDetectiveAgency.Services.Interfaces
{
    public interface ICaseService
    {
        Task<List<CaseListDto>> GetAvailableCasesAsync();
        Task<CaseDetailDto?> GetCaseDetailsAsync(int caseId);
        Task<CaseAttemptResultDto> SubmitAccusationAsync(string userId, SubmitAccusationDto accusationDto);
    }
}
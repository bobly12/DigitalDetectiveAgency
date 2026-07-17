using DigitalDetectiveAgency.Models.DTOs.Case;

namespace DigitalDetectiveAgency.ViewModels
{
    public class CaseDetailsViewModel
    {
        public CaseDetailDto CaseDetails { get; set; } = null!;
        public bool AlreadyAttempted { get; set; }
        public bool WasPerfectScore { get; set; }
        public int BestScoreEarned { get; set; }
    }
}
using DigitalDetectiveAgency.Models.DTOs.Case;

namespace DigitalDetectiveAgency.ViewModels
{
    public class DashboardViewModel
    {
        public string PlayerName { get; set; } = string.Empty;
        public int TotalCasesAttempted { get; set; }
        public int TotalCasesSolved { get; set; }
        public int TotalScore { get; set; }
        
        // Calculated property for display accuracy
        public double SuccessRate => TotalCasesAttempted > 0 
            ? Math.Round((double)TotalCasesSolved / TotalCasesAttempted * 100, 1) 
            : 0;

        // A small list of featured/recent cases to display on the dashboard home
        public List<CaseListDto> RecentCases { get; set; } = new();
    }
}
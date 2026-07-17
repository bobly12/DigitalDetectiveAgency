using DigitalDetectiveAgency.Models.DTOs.Case;

namespace DigitalDetectiveAgency.ViewModels
{
    public class CaseListViewModel
    {
        public List<CaseListDto> Cases { get; set; } = new();
        public string? SearchTerm { get; set; }
        public string? SelectedDifficulty { get; set; }
    }
}
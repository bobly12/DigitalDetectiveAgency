using DigitalDetectiveAgency.Models.DTOs.Evidence;
using DigitalDetectiveAgency.Models.DTOs.Witness;

namespace DigitalDetectiveAgency.Models.DTOs.Case
{
    public class CaseDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public List<SuspectDto> Suspects { get; set; } = new();
        public List<EvidenceDto> Evidences { get; set; } = new();
        public List<WitnessDto> Witnesses { get; set; } = new();
    }

    public class SuspectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        // Add this line to satisfy the Razor View compiler:
        public string AlibiSummary { get; set; } = string.Empty; 
    }
}
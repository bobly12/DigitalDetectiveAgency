using DigitalDetectiveAgency.Models.DTOs.Evidence;
using DigitalDetectiveAgency.Models.DTOs.Witness;
using DigitalDetectiveAgency.Models.DTOs.Suspect;

namespace DigitalDetectiveAgency.Models.DTOs.Case
{
    public class CaseDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string VictimName { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;

        public List<EvidenceDto> Evidences { get; set; } = new List<EvidenceDto>();
        public List<WitnessDto> Witnesses { get; set; } = new List<WitnessDto>();
        public List<SuspectDto> Suspects { get; set; } = new List<SuspectDto>();
    }
}
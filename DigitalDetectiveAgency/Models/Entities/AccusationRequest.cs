using System.ComponentModel.DataAnnotations;

namespace DigitalDetectiveAgency.Models.Entities
{
    public class AccusationRequest
    {
        [Required] 
        public int CaseId { get; init; }

        [Required] 
        public int SuspectId { get; init; }

        [Required] 
        public int EvidenceId { get; init; }

        [Required] 
        public int WitnessId { get; init; }
    }
}
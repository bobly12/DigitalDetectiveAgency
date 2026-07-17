using System.ComponentModel.DataAnnotations;

namespace DigitalDetectiveAgency.Models.Entities
{
    public class AccusationRequest
    {
        [Required] public int CaseId { get; set; }
        [Required] public int SuspectId { get; set; }
        [Required] public int EvidenceId { get; set; }
        [Required] public int WitnessId { get; set; }
    }
}
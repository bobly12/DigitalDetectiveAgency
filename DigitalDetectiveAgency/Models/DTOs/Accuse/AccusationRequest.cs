using System.ComponentModel.DataAnnotations;

public class AccusationRequest
{
    [Required]
    public int CaseId { get; set; }

    [Required]
    public int SuspectId { get; set; }

    [Required]
    public int EvidenceId { get; set; }

    [Required]
    public int WitnessId { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace DigitalDetectiveAgency.Models.DTOs.CaseAttempt
{
    public class SubmitAccusationDto
    {
        [Required]
        public int CaseId { get; set; }

        [Required]
        public int SuspectId { get; set; }
    }
}
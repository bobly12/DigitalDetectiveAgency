using System.ComponentModel.DataAnnotations;

namespace DigitalDetectiveAgency.Models.Entities
{
    public class CaseAttempt
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int CaseId { get; set; }

        [Required]
        public int AccusedSuspectId { get; set; }

        public bool IsCorrect { get; set; }

        public int Score { get; set; }

        public DateTime AttemptedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public ApplicationUser User { get; set; } = null!;
        public Case Case { get; set; } = null!;
        public Suspect AccusedSuspect { get; set; } = null!;
    }
}
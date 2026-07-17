namespace DigitalDetectiveAgency.Models.Entities
{
    public class CaseAttempt
    {
        public int Id { get; init; }
        public int CaseId { get; init; }
        
        public string UserId { get; init; } = string.Empty; 
        public ApplicationUser? User { get; init; } 
        
        public int AccusedSuspectId { get; init; }
        public Suspect? AccusedSuspect { get; init; }

        public int SubmittedEvidenceId { get; init; }
        public int SubmittedWitnessId { get; init; }

        public bool IsCorrect { get; init; }
        public int ScoreAwarded { get; init; }
        public DateTime AttemptedAt { get; init; } = DateTime.UtcNow;

        public Case? Case { get; init; }
    }
}
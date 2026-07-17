namespace DigitalDetectiveAgency.Models.DTOs.CaseAttempt
{
    public class CaseAttemptResultDto
    {
        public bool IsCorrect { get; set; }
        public int ScoreEarned { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime AttemptedAt { get; set; }
    }
}
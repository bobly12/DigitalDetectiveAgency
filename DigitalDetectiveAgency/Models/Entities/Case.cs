using System.ComponentModel.DataAnnotations;

namespace DigitalDetectiveAgency.Models.Entities
{
    public class Case
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string VictimName { get; set; } = string.Empty;

        [Required]
        public Difficulty Difficulty { get; set; }

        // The answer key connection
        public int CorrectSuspectId { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public ICollection<Evidence> Evidences { get; set; } = new List<Evidence>();
        public ICollection<Witness> Witnesses { get; set; } = new List<Witness>();
        public ICollection<Suspect> Suspects { get; set; } = new List<Suspect>();
        public ICollection<CaseAttempt> CaseAttempts { get; set; } = new List<CaseAttempt>();
    }
}
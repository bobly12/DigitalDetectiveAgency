using System.ComponentModel.DataAnnotations;

namespace DigitalDetectiveAgency.Models.Entities
{
    public class Suspect
    {
        public int Id { get; set; }

        [Required]
        public int CaseId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Motive { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Alibi { get; set; } = string.Empty;

        public bool IsGuilty { get; set; } = false;

        // Navigation Properties
        public Case Case { get; set; } = null!;
        public ICollection<CaseAttempt> CaseAttempts { get; set; } = new List<CaseAttempt>();
    }
}
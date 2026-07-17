using System.ComponentModel.DataAnnotations;

namespace DigitalDetectiveAgency.Models.Entities
{
    public class Evidence
    {
        public int Id { get; set; }

        [Required]
        public int CaseId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        public bool IsKeyEvidence { get; set; } = false;

        // Navigation Property
        public Case Case { get; set; } = null!;
    }
}
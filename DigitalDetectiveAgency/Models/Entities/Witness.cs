using System.ComponentModel.DataAnnotations;

namespace DigitalDetectiveAgency.Models.Entities
{
    public class Witness
    {
        public int Id { get; set; }

        [Required]
        public int CaseId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(1500)]
        public string Statement { get; set; } = string.Empty;

        public bool IsReliable { get; set; } = true;

        // Navigation Property
        public Case Case { get; set; } = null!;
    }
}
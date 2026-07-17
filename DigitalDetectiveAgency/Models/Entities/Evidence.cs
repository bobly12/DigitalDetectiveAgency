using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalDetectiveAgency.Models.Entities
{
    public class Evidence
    {
        public int Id { get; init; }

        [Required]
        public int CaseId { get; init; }

        [Required]
        [StringLength(100)]
        public string Title { get; init; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; init; } = string.Empty;

        public bool IsKeyEvidence { get; init; }

        public Case Case { get; init; } = null!;
        
        public int? ContradictsWitnessId { get; init; }
    
        [ForeignKey(nameof(ContradictsWitnessId))]
        public Witness? ContradictsWitness { get; init; }
    }
}
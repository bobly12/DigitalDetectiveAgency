using DigitalDetectiveAgency.Models.Entities;

namespace DigitalDetectiveAgency.Models.DTOs.Case
{
    public class CaseListDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string VictimName { get; set; } = string.Empty;
        public Difficulty Difficulty { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

namespace DigitalDetectiveAgency.Models.DTOs.Suspect
{
    public class SuspectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Motive { get; set; } = string.Empty;
        public string Alibi { get; set; } = string.Empty;
    }
}
namespace DigitalDetectiveAgency.Models.DTOs
{
    public class UserAuthResponseDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty; 
    }
}
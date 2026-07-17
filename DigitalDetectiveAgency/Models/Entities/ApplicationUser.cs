using Microsoft.AspNetCore.Identity;

namespace DigitalDetectiveAgency.Models.Entities
{
    /// <summary>
    /// Extends the default ASP.NET Core IdentityUser with custom player profile metrics.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string DisplayName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int CasesSolved { get; set; } = 0;

        public int CasesAttempted { get; set; } = 0;
    }
}
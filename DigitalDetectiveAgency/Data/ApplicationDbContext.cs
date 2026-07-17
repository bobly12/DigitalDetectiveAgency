using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DigitalDetectiveAgency.Models.Entities;

namespace DigitalDetectiveAgency.Data
{
    /// <summary>
    /// The primary Entity Framework Core database context.
    /// Inherits from IdentityDbContext to include all standard ASP.NET Core Identity security tables.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // We will uncomment these DbSets in Step 6 once the domain entity classes are created!
        public DbSet<Case> Cases { get; set; } = null!;
        public DbSet<Evidence> Evidences { get; set; } = null!;
        public DbSet<Witness> Witnesses { get; set; } = null!;
        public DbSet<Suspect> Suspects { get; set; } = null!;
        public DbSet<CaseAttempt> CaseAttempts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // CRITICAL: Always call the base Identity configuration first.
            // If omitted, Identity table relationships will break during migrations.
            base.OnModelCreating(builder);

            // Dynamically applies all Fluent API configurations from Data/Configurations/
            // This is where we will safely hook up the circular Case <-> Suspect relationship in Step 5.
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DigitalDetectiveAgency.Models.Entities;

namespace DigitalDetectiveAgency.Data.Configurations
{
    public class SuspectConfiguration : IEntityTypeConfiguration<Suspect>
    {
        public void Configure(EntityTypeBuilder<Suspect> builder)
        {
            builder.HasKey(s => s.Id);

            // Standard relationship: One Case can have multiple Suspects.
            // If a Case is deleted, its Suspect list is safely cascading deleted.
            builder.HasOne(s => s.Case)
                .WithMany(c => c.Suspects)
                .HasForeignKey(s => s.CaseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
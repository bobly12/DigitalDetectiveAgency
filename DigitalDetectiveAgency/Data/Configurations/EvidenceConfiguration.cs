using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DigitalDetectiveAgency.Models.Entities;

namespace DigitalDetectiveAgency.Data.Configurations
{
    public class EvidenceConfiguration : IEntityTypeConfiguration<Evidence>
    {
        public void Configure(EntityTypeBuilder<Evidence> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Case)
                .WithMany(c => c.Evidences)
                .HasForeignKey(e => e.CaseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
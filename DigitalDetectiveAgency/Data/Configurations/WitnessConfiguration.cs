using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DigitalDetectiveAgency.Models.Entities;

namespace DigitalDetectiveAgency.Data.Configurations
{
    public class WitnessConfiguration : IEntityTypeConfiguration<Witness>
    {
        public void Configure(EntityTypeBuilder<Witness> builder)
        {
            builder.HasKey(w => w.Id);

            builder.HasOne(w => w.Case)
                .WithMany(c => c.Witnesses)
                .HasForeignKey(w => w.CaseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
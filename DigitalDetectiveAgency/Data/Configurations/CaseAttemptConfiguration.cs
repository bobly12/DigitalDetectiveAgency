using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DigitalDetectiveAgency.Models.Entities;

namespace DigitalDetectiveAgency.Data.Configurations
{
    public class CaseAttemptConfiguration : IEntityTypeConfiguration<CaseAttempt>
    {
        public void Configure(EntityTypeBuilder<CaseAttempt> builder)
        {
            builder.HasKey(ca => ca.Id);

            // Connects User to their attempts
            builder.HasOne(ca => ca.User)
                .WithMany()
                .HasForeignKey(ca => ca.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Connects Case to its history of attempts
            builder.HasOne(ca => ca.Case)
                .WithMany(c => c.CaseAttempts)
                .HasForeignKey(ca => ca.CaseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Tracks which suspect was accused during this specific attempt
            builder.HasOne(ca => ca.AccusedSuspect)
                .WithMany(s => s.CaseAttempts)
                .HasForeignKey(ca => ca.AccusedSuspectId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
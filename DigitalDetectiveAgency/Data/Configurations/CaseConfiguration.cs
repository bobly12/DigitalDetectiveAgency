using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DigitalDetectiveAgency.Models.Entities;


namespace DigitalDetectiveAgency.Data.Configurations
{
   public class CaseConfiguration : IEntityTypeConfiguration<Case>
   {
       public void Configure(EntityTypeBuilder<Case> builder)
       {
           builder.HasKey(c => c.Id);


           // CRITICAL: Handle the circular reference to Suspect
           // A Case has an answer key pointing to a Suspect, but a Suspect belongs to a Case.
           // Turning off cascade delete here prevents SQL Server deployment loops.
           builder.HasOne<Suspect>()
               .WithMany()
               .HasForeignKey(c => c.CorrectSuspectId)
               .OnDelete(DeleteBehavior.Restrict);


           // Map standard Enum to an integer column in SQL Server
           builder.Property(c => c.Difficulty)
               .HasConversion<int>()
               .IsRequired();
       }
   }
}

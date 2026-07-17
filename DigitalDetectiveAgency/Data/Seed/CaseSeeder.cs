using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DigitalDetectiveAgency.Data;
using DigitalDetectiveAgency.Models.Entities;

namespace DigitalDetectiveAgency.Data.Seed
{
    public static class CaseSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // 1. SEED SECURITY ROLES
            string[] roles = { "Admin", "Player" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 2. SEED ADMIN USER
            string adminEmail = "admin@detective.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    DisplayName = "Head Detective (Admin)",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(adminUser, "Admin123!");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // 3. SEED THE DETECTIVE CASES (Only if database is empty)
            if (!await context.Cases.AnyAsync())
            {
                await using var transaction = await context.Database.BeginTransactionAsync();

                // ==========================================
                // CASE 1: BEGINNER - The Smart-Home Blackout
                // ==========================================
                var case1 = new Case
                {
                    Title = "The Smart-Home Blackout",
                    Description = "Tech billionaire Arthur Sterling was locked out of his smart mansion's master bedroom while a rogue script completely wiped his off-grid cold storage drive containing $2M in crypto assets. The perpetrator overrode the home server from an internal physical port, forcing a local power grid loop.",
                    VictimName = "Arthur Sterling",
                    Difficulty = Difficulty.Beginner,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-10)
                };

                var c1Sus1 = new Suspect { Name = "Elena Vance", Motive = "Lead automation engineer who was passed over for a major promotion last month.", Alibi = "Claimed she was working remotely from a cafe downtown all night.", IsGuilty = false };
                var c1Sus2 = new Suspect { Name = "Marcus 'Sparky' Brody", Motive = "Private electrician hired to upgrade the home solar grid; deep in gambling debt.", Alibi = "Stated he left the property at 6:00 PM before the blackout.", IsGuilty = true };

                case1.Suspects = new List<Suspect> { c1Sus1, c1Sus2 };
                case1.Evidences = new List<Evidence> 
                { 
                    new Evidence { Title = "Modified RJ45 Rubber Ducky", Description = "A keystroke injection tool found hidden inside the wall-mounted thermostat panel.", IsKeyEvidence = true } 
                };
                case1.Witnesses = new List<Witness> 
                { 
                    new Witness { Name = "The Butler", Statement = "I saw Mr. Brody lingering near the server closet with a small black pouch around 7:30 PM.", IsReliable = true } 
                };

                // Temporarily disable constraint to plant circular dependencies safely
                await context.Database.ExecuteSqlRawAsync("ALTER TABLE Cases NOCHECK CONSTRAINT FK_Cases_Suspects_CorrectSuspectId");
                
                await context.Cases.AddAsync(case1);
                await context.SaveChangesAsync();

                // Assign answer key reference now that IDs are generated
                case1.CorrectSuspectId = c1Sus2.Id;
                await context.SaveChangesAsync();
                
                await context.Database.ExecuteSqlRawAsync("ALTER TABLE Cases CHECK CONSTRAINT FK_Cases_Suspects_CorrectSuspectId");

                await transaction.CommitAsync();
                Console.WriteLine("🕵️‍♂️ [CASE SEEDER]: Core game data seeded successfully.");
            }
        }
    }
}
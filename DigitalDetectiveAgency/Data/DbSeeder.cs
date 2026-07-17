using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DigitalDetectiveAgency.Models.Entities;

namespace DigitalDetectiveAgency.Data;

public static class DbSeeder
{
    public static async Task SeedDataAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        try
        {
            Console.WriteLine("🕵️‍♂️ [SEEDER]: Applying migrations...");
            await context.Database.MigrateAsync();

            // 1. Seed Roles
            string[] roles = ["Admin", "Player"];
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 🔥 FORCE CLEAN OLD SEED DATA (Fixed Sequence)
            if (context.Cases.Any())
            {
                Console.WriteLine("🕵️‍♂️ [SEEDER]: Cleaning up old, unlinked data records...");
                
                // 1. Clear out child tracking logs first to resolve database references
                if (context.Set<CaseAttempt>().Any())
                {
                    context.Set<CaseAttempt>().RemoveRange(context.Set<CaseAttempt>());
                    await context.SaveChangesAsync();
                }

                // 2. Now it is completely safe to drop parent case rows
                context.Cases.RemoveRange(context.Cases);
                await context.SaveChangesAsync();
            }

            // 2. Seed Cases with full relationship chains
            if (!context.Cases.Any())
            {
                Console.WriteLine("🕵️‍♂️ [SEEDER]: Database clean. Planting custom cases and suspects...");

                await using var transaction = await context.Database.BeginTransactionAsync();

                // --- CASE 1 SETUP ---
                var case1 = new Case
                {
                    Title = "The Missing Source Code Ledger",
                    Description = "A high-profile rival tech firm has acquired a decrypted copy of a secret ledger. Find the leak.",
                    VictimName = "TechCorp CEO",
                    Difficulty = Difficulty.Beginner,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var suspect1 = new Suspect { Name = "Malicious Mike", Motive = "Corporate espionage payout.", Alibi = "Was checking code logs.", IsGuilty = true };
                var suspect2 = new Suspect { Name = "Innocent Iris", Motive = "Disgruntled worker.", Alibi = "Was at lunch.", IsGuilty = false };

                case1.Suspects = [suspect1, suspect2];
                case1.Evidences = [new Evidence { Title = "USB Drive", Description = "Found near the server room.", IsKeyEvidence = true }];
                case1.Witnesses = [new Witness { Name = "Janitor Joe", Statement = "Saw someone running out at midnight.", IsReliable = true }];

                await context.Database.ExecuteSqlRawAsync("ALTER TABLE Cases NOCHECK CONSTRAINT FK_Cases_Suspects_CorrectSuspectId");
                await context.Cases.AddAsync(case1);
                await context.SaveChangesAsync();

                case1.CorrectSuspectId = suspect1.Id;
                await context.SaveChangesAsync();
                await context.Database.ExecuteSqlRawAsync("ALTER TABLE Cases CHECK CONSTRAINT FK_Cases_Suspects_CorrectSuspectId");

                // --- CASE 2 SETUP ---
                var case2 = new Case
                {
                    Title = "The Midnight Phishing Heist",
                    Description = "Someone drained the agency's crypto escrow wallet using an advanced session hijacking script.",
                    VictimName = "Crypto Exchange Manager",
                    Difficulty = Difficulty.Intermediate,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var suspect3 = new Suspect { Name = "Phishing Phil", Motive = "Debt collections.", Alibi = "At home sleeping.", IsGuilty = true };
                case2.Suspects = [suspect3];
                case2.Evidences = [new Evidence { Title = "Spoofed Email", Description = "Sent from a hidden proxy server.", IsKeyEvidence = true }];

                await context.Database.ExecuteSqlRawAsync("ALTER TABLE Cases NOCHECK CONSTRAINT FK_Cases_Suspects_CorrectSuspectId");
                await context.Cases.AddAsync(case2);
                await context.SaveChangesAsync();

                case2.CorrectSuspectId = suspect3.Id;
                await context.SaveChangesAsync();
                await context.Database.ExecuteSqlRawAsync("ALTER TABLE Cases CHECK CONSTRAINT FK_Cases_Suspects_CorrectSuspectId");

                await transaction.CommitAsync();
                Console.WriteLine("🕵️‍♂️ [SEEDER]: 2 fully linked cases injected safely!");
            }
            else
            {
                Console.WriteLine("🕵️‍♂️ [SEEDER]: Data already exists.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ [SEEDER ERROR]: {ex.Message}");
        }
    }
}
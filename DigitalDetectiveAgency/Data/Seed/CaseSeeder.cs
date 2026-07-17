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

            // 3. SEED THE 3 DETECTIVE CASES (Only if database is empty)
            if (!await context.Cases.AnyAsync())
            {
                // ==========================================
                // CASE 1: BEGINNER - The Smart-Home Blackout
                // ==========================================
                var case1 = new Case
                {
                    Title = "The Smart-Home Blackout",
                    Description = "Tech billionaire Arthur Sterling was locked out of his smart mansion's master bedroom while a rogue script completely wiped his off-grid cold storage drive containing $2M in crypto assets. The perpetrator overrode the home server from an internal physical port, forcing a local power grid loop.",
                    VictimName = "Arthur Sterling",
                    Difficulty = Difficulty.Beginner,
                    CreatedAt = DateTime.UtcNow.AddDays(-10)
                };

                var c1Sus1 = new Suspect { Name = "Elena Vance", Motive = "Lead automation engineer who was passed over for a major promotion last month.", Alibi = "Claimed she was working remotely from a cafe downtown all night.", IsGuilty = false };
                var c1Sus2 = new Suspect { Name = "Marcus 'Zero' Hayes", Motive = "Former systems contractor fired for installing unauthorized network monitoring tools.", Alibi = "Claims he was sleeping off a double shift at his new apartment across town.", IsGuilty = true };
                var c1Sus3 = new Suspect { Name = "Chloe Sterling", Motive = "Arthur's tech-influencer daughter facing massive personal debts.", Alibi = "Was live-streaming to her followers from the mansion's guest wing during the blackout.", IsGuilty = false };

                case1.Suspects.Add(c1Sus1);
                case1.Suspects.Add(c1Sus2);
                case1.Suspects.Add(c1Sus3);

                case1.Evidences.Add(new Evidence { Title = "Modified Raspberry Pi 4", Description = "Found physically taped underneath the main server rack. It was running a brute-force terminal script targeting the local root admin panel.", IsKeyEvidence = true });
                case1.Evidences.Add(new Evidence { Title = "Discarded Access Badge", Description = "A master keycard dropped near the basement stairs, cleanly wiped of fingerprints but authorized at 11:42 PM.", IsKeyEvidence = false });
                case1.Evidences.Add(new Evidence { Title = "Burnt Network Cable", Description = "A standard Cat6 Ethernet line that melted during the forced electrical loop on the main switch.", IsKeyEvidence = false });

                case1.Witnesses.Add(new Witness { Name = "David (Mansion Butler)", Statement = "I saw a shadowy figure wearing a dark contractor hoodie near the basement entrance right before the smart lights flickered out.", IsReliable = true });
                case1.Witnesses.Add(new Witness { Name = "Cafe Barista", Statement = "Elena Vance was definitely in our downtown shop. She ordered a triple espresso at 10:00 PM and kept typing furiously on her laptop until we closed at midnight.", IsReliable = true });

                context.Cases.Add(case1);
                await context.SaveChangesAsync(); // Saves case1 + children to generate IDs

                // Link the answer key
                case1.CorrectSuspectId = c1Sus2.Id;

                // ==========================================
                // CASE 2: INTERMEDIATE - The Cryptic Commit
                // ==========================================
                var case2 = new Case
                {
                    Title = "The Cryptic Commit",
                    Description = "Hours before the highly anticipated deployment of NexaCorp's automated trading algorithm, a malicious back-door commit was pushed directly to production. The exploit intentionally shorted domestic airline assets, draining $10M from corporate reserves within minutes.",
                    VictimName = "NexaCorp Trading Firm",
                    Difficulty = Difficulty.Intermediate,
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                };

                var c2Sus1 = new Suspect { Name = "Liam Chen", Motive = "Senior DevOps architect who vocalized open opposition to the algorithm's aggressive deployment timeline.", Alibi = "Maintains his corporate hardware token was lost on the subway earlier that morning.", IsGuilty = false };
                var c2Sus2 = new Suspect { Name = "Sarah Jenkins", Motive = "Junior software engineer eager to clear a massive margin-call debt on her personal trading portfolio.", Alibi = "Her Git account signature was used to sign and approve the rogue production code branch.", IsGuilty = false };
                var c2Sus3 = new Suspect { Name = "Victor Vance", Motive = "Lead Security Auditor bribed by a rival trading firm to orchestrate a catastrophic PR disaster.", Alibi = "Was physically running an active network vulnerability scan during the exact window of the breach.", IsGuilty = true };

                case2.Suspects.Add(c2Sus1);
                case2.Suspects.Add(c2Sus2);
                case2.Suspects.Add(c2Sus3);

                case2.Evidences.Add(new Evidence { Title = "Hardware Token Decryptor", Description = "A custom-built USB interception device recovered directly from the Network Operations Center floor, configured to mirror security tokens.", IsKeyEvidence = true });
                case2.Evidences.Add(new Evidence { Title = "Leaked Git Logs", Description = "Commit history displaying Sarah Jenkins' cryptographic GPG signature attached directly to the rogue back-door payload.", IsKeyEvidence = false });
                case2.Evidences.Add(new Evidence { Title = "Encrypted Foreign Wire Transfer", Description = "An anonymous, outbound ledger transfer tracing back to an offshore account, initialized shortly after the short positions cleared.", IsKeyEvidence = false });

                case2.Witnesses.Add(new Witness { Name = "Security Guard", Statement = "I saw Sarah Jenkins looking highly panicked near the server room around 2:00 PM, though she claimed she was just looking for a lost phone.", IsReliable = false }); // Unreliable red herring
                case2.Witnesses.Add(new Witness { Name = "IT Network Admin", Statement = "The rogue connection bypassed our external firewall seamlessly because it originated from an internal white-listed IP usually reserved for security auditing.", IsReliable = true });

                context.Cases.Add(case2);
                await context.SaveChangesAsync();

                case2.CorrectSuspectId = c2Sus3.Id;

                // ==========================================
                // CASE 3: ADVANCED - Phantom Signal Heist
                // ==========================================
                var case3 = new Case
                {
                    Title = "The Phantom Signal Heist",
                    Description = "The central laboratory of Apex Robotics was breached over a satellite data uplink. Their proprietary neural-network design files were completely exfiltrated. The thief used an advanced automated proxy to scrub server logs, leaving behind an incredibly complex, timed digital alibi.",
                    VictimName = "Dr. Aris Thorne",
                    Difficulty = Difficulty.Advanced,
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                };

                var c3Sus1 = new Suspect { Name = "Dr. Maya Lin", Motive = "Co-founder who believes Apex Robotics is irresponsibly commercializing her dangerous AI research.", Alibi = "Her private key logged a successful user authentication session from a research station based in London during the precise time of the heist.", IsGuilty = false };
                var c3Sus2 = new Suspect { Name = "Ray Vance", Motive = "Disgruntled infrastructure engineer blackmailed by an overseas tech syndicate to download core project data.", Alibi = "Digital access logs place his account fully offline, with his physical keycard swiped out of the facility hours prior.", IsGuilty = true };
                var c3Sus3 = new Suspect { Name = "CEO Rebecca Vance", Motive = "Seeking to stage a massive corporate insurance fraud payout to offset severe quarterly losses.", Alibi = "Was hosting a major live corporate press conference with over 200 physical attendees.", IsGuilty = false };

                case3.Suspects.Add(c3Sus1);
                case3.Suspects.Add(c3Sus2);
                case3.Suspects.Add(c3Sus3);

                case3.Evidences.Add(new Evidence { Title = "Cron-Job Scheduling Script", Description = "A hidden server automated script found scheduled to run an exfiltration wrapper, configured to execute under Ray's offline credentials hours after he left the building.", IsKeyEvidence = true });
                case3.Evidences.Add(new Evidence { Title = "Spoofed Satellite Node Log", Description = "Raw packet stream traces indicating that the connection from London was actually a routed VPN tunnel bouncing off an insecure local node.", IsKeyEvidence = false });
                case3.Evidences.Add(new Evidence { Title = "Recovered Micro-SD Card", Description = "An encrypted storage chip dropped in the main parking garage, containing structural source files labeled 'Project Apex'.", IsKeyEvidence = false });

                case3.Witnesses.Add(new Witness { Name = "Network Specialist", Statement = "The file transfer didn't happen in real-time. It was heavily compressed and pulled using an automated macro script configured days in advance.", IsReliable = true });
                case3.Witnesses.Add(new Witness { Name = "Night Janitor", Statement = "I saw Ray Vance hovering around the primary terminal long after his official shift ended, typing rapidly while constantly looking over his shoulder.", IsReliable = true });

                context.Cases.Add(case3);
                await context.SaveChangesAsync();

                case3.CorrectSuspectId = c3Sus2.Id;
                await context.SaveChangesAsync();
            }
        }
    }
}
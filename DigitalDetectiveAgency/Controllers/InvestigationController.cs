using System;
using System.Security.Claims; // Required for FindFirstValue
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DigitalDetectiveAgency.Data;
using DigitalDetectiveAgency.Models.Entities;

namespace DigitalDetectiveAgency.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InvestigationController(ApplicationDbContext context) : ControllerBase
    {
        [HttpGet("status")]
        public IActionResult GetStatus() => 
            Ok(new { system = "Investigation processing engine fully online." });

        [HttpPost("accuse")]
        public async Task<IActionResult> Accuse([FromBody] AccusationRequest request)
        {
            var currentCase = await context.Cases
                .FirstOrDefaultAsync(c => c.Id == request.CaseId);

            if (currentCase == null) 
            {
                return NotFound(new { error = "Case file missing from database archives." });
            }

            var chosenEvidence = await context.Evidences
                .FirstOrDefaultAsync(e => e.Id == request.EvidenceId && e.CaseId == request.CaseId);

            bool suspectMatches = currentCase.CorrectSuspectId == request.SuspectId;
            bool logicMatches = chosenEvidence != null && chosenEvidence.ContradictsWitnessId == request.WitnessId;
            bool finalSuccess = suspectMatches && logicMatches;

            int score = finalSuccess ? 1000 : 0; 

            // Fixed: Replaced old claim mapping with the modern, non-deprecated symbol extension method
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Guest";

            var attempt = new CaseAttempt
            {
                CaseId = request.CaseId,
                UserId = currentUserId,
                AccusedSuspectId = request.SuspectId,
                SubmittedEvidenceId = request.EvidenceId,
                SubmittedWitnessId = request.WitnessId,
                IsCorrect = finalSuccess,
                ScoreAwarded = score,
                AttemptedAt = DateTime.UtcNow
            };

            context.CaseAttempts.Add(attempt);
            await context.SaveChangesAsync();

            if (finalSuccess)
            {
                return Ok(new
                {
                    success = true,
                    attemptId = attempt.Id,
                    score,
                    message = "Arrest mandate approved. You successfully exposed the contradiction loops in the target's alibi."
                });
            }

            return BadRequest(new
            {
                success = false,
                attemptId = attempt.Id,
                message = "The suspect's legal counsel broke your argument. Your evidence failed to directly expose a logical lie."
            });
        }
    }
}
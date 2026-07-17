using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Models.DTOs.Case;
using DigitalDetectiveAgency.Models.DTOs.CaseAttempt;
using DigitalDetectiveAgency.Services.Interfaces;
using DigitalDetectiveAgency.Repositories.Interfaces;
using AutoMapper;

namespace DigitalDetectiveAgency.Services.Implementations
{
    // Fixed: Clean Primary Constructor replaces boilerplate injection fields
    public class CaseService(
        ICaseRepository caseRepository,
        ICaseAttemptRepository caseAttemptRepository,
        IScoringService scoringService,
        IMapper mapper) : ICaseService
    {
        public async Task<List<CaseListDto>> GetAvailableCasesAsync()
        {
            var cases = await caseRepository.GetActiveCasesAsync();
            return mapper.Map<List<CaseListDto>>(cases);
        }

        public async Task<CaseDetailDto?> GetCaseDetailsAsync(int caseId)
        {
            var caseEntity = await caseRepository.GetByIdWithDetailsAsync(caseId);
            return caseEntity == null ? null : mapper.Map<CaseDetailDto>(caseEntity);
        }

        public async Task<CaseAttemptResultDto> SubmitAccusationAsync(string userId, SubmitAccusationDto accusationDto)
        {
            var existingAttempt = await caseAttemptRepository.GetExistingAttemptAsync(userId, accusationDto.CaseId);
            if (existingAttempt != null)
            {
                throw new InvalidOperationException("You have already attempted this case.");
            }

            var caseWithDetails = await caseRepository.GetByIdWithDetailsAsync(accusationDto.CaseId);
            if (caseWithDetails == null)
            {
                throw new KeyNotFoundException("Case not found.");
            }

            bool suspectExists = caseWithDetails.Suspects.Any(s => s.Id == accusationDto.SuspectId);
            if (!suspectExists)
            {
                throw new InvalidOperationException("Selected suspect does not belong to this case.");
            }

            bool isCorrect = caseWithDetails.CorrectSuspectId == accusationDto.SuspectId;
            int score = scoringService.CalculateScore(caseWithDetails.Difficulty, isCorrect);

            // Fixed: Standardized object properties to use ScoreAwarded mapping mechanics
            var attempt = new CaseAttempt
            {
                UserId = userId,
                CaseId = accusationDto.CaseId,
                AccusedSuspectId = accusationDto.SuspectId,
                IsCorrect = isCorrect,
                ScoreAwarded = score,
                AttemptedAt = DateTime.UtcNow
            };

            await caseAttemptRepository.AddAsync(attempt);

            return new CaseAttemptResultDto
            {
                IsCorrect = isCorrect,
                ScoreEarned = score,
                Message = isCorrect ? "Case Solved!" : "Incorrect suspect. Keep investigating!",
                AttemptedAt = attempt.AttemptedAt
            };
        }
    }
}
using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Models.DTOs.Case;
using DigitalDetectiveAgency.Models.DTOs.CaseAttempt;
using DigitalDetectiveAgency.Services.Interfaces;
using DigitalDetectiveAgency.Repositories.Interfaces;
using AutoMapper;

namespace DigitalDetectiveAgency.Services.Implementations
{
    public class CaseService : ICaseService
    {
        private readonly ICaseRepository _caseRepository;
        private readonly ICaseAttemptRepository _caseAttemptRepository;
        private readonly IScoringService _scoringService;
        private readonly IMapper _mapper;

        public CaseService(
            ICaseRepository caseRepository,
            ICaseAttemptRepository caseAttemptRepository,
            IScoringService scoringService,
            IMapper mapper)
        {
            _caseRepository = caseRepository;
            _caseAttemptRepository = caseAttemptRepository;
            _scoringService = scoringService;
            _mapper = mapper;
        }

        public async Task<List<CaseListDto>> GetAvailableCasesAsync()
        {
            var cases = await _caseRepository.GetActiveCasesAsync();
            return _mapper.Map<List<CaseListDto>>(cases);
        }

        public async Task<CaseDetailDto?> GetCaseDetailsAsync(int caseId)
        {
            var caseEntity = await _caseRepository.GetByIdWithDetailsAsync(caseId);

            if (caseEntity == null)
                return null;

            return _mapper.Map<CaseDetailDto>(caseEntity);
        }

        public async Task<CaseAttemptResultDto> SubmitAccusationAsync(
            string userId,
            SubmitAccusationDto accusationDto)
        {
            // 1. Prevent duplicate submissions
            var existingAttempt =
                await _caseAttemptRepository.GetExistingAttemptAsync(
                    userId,
                    accusationDto.CaseId);

            if (existingAttempt != null)
            {
                throw new InvalidOperationException(
                    "You have already attempted this case.");
            }

            // 2. Load full case details
            var caseWithDetails =
                await _caseRepository.GetByIdWithDetailsAsync(
                    accusationDto.CaseId);

            if (caseWithDetails == null)
            {
                throw new KeyNotFoundException(
                    "Case not found.");
            }

            // 3. Verify suspect belongs to this case
            bool suspectExists =
                caseWithDetails.Suspects.Any(
                    s => s.Id == accusationDto.SuspectId);

            if (!suspectExists)
            {
                throw new InvalidOperationException(
                    "Selected suspect does not belong to this case.");
            }

            // 4. Determine if accusation is correct
            bool isCorrect =
                caseWithDetails.CorrectSuspectId ==
                accusationDto.SuspectId;

            // 5. Calculate score
            int score =
                _scoringService.CalculateScore(
                    caseWithDetails.Difficulty,
                    isCorrect);

            // 6. Create attempt record
            var attempt = new CaseAttempt
            {
                UserId = userId,
                CaseId = accusationDto.CaseId,
                AccusedSuspectId = accusationDto.SuspectId,
                IsCorrect = isCorrect,
                Score = score,
                AttemptedAt = DateTime.UtcNow
            };

            // 7. Save attempt
            await _caseAttemptRepository.AddAsync(attempt);

            // 8. Return result
            return new CaseAttemptResultDto
            {
                IsCorrect = isCorrect,
                ScoreEarned = score,
                Message = isCorrect
                    ? "Case Solved!"
                    : "Incorrect suspect. Keep investigating!",
                AttemptedAt = attempt.AttemptedAt
            };
        }
    }
}
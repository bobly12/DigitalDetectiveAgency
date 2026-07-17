using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Services.Interfaces;

namespace DigitalDetectiveAgency.Services.Implementations
{
    public class ScoringService : IScoringService
    {
        public int CalculateScore(Difficulty difficulty, bool isCorrect)
        {
            if (!isCorrect) return 0;
            
            return difficulty switch
            {
                Difficulty.Beginner => 100,
                Difficulty.Intermediate => 250,
                Difficulty.Advanced => 500,
                _ => 0
            };
        }
    }
}
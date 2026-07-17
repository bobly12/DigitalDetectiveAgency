using DigitalDetectiveAgency.Models.Entities;

namespace DigitalDetectiveAgency.Services.Interfaces
{
    public interface IScoringService
    {
        int CalculateScore(Difficulty difficulty, bool isCorrect);
    }
}
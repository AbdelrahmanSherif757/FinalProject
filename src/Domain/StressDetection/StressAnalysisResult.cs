using SharedKernel;
using System.Text.Json;

namespace Domain.StressDetection;

/// <summary>
/// Represents the result of stress analysis performed on keyboard data.
/// </summary>
public sealed class StressAnalysisResult : Entity
{
    public Guid Id { get; private set; }

    /// <summary>
    /// The user this analysis belongs to.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// The stress data that was analyzed.
    /// </summary>
    public Guid StressDataId { get; private set; }

    /// <summary>
    /// The detected stress level.
    /// </summary>
    public StressLevel DetectedStressLevel { get; private set; }

    /// <summary>
    /// Confidence score of the analysis (0-100).
    /// </summary>
    public double ConfidenceScore { get; private set; }

    /// <summary>
    /// Raw stress score before classification (0-100).
    /// </summary>
    public double RawStressScore { get; private set; }

    /// <summary>
    /// JSON containing component scores breakdown.
    /// </summary>
    public string ComponentScoresJson { get; private set; } = string.Empty;

    /// <summary>
    /// Detailed analysis message explaining the result.
    /// </summary>
    public string AnalysisMessage { get; private set; } = string.Empty;

    /// <summary>
    /// Recommended action for the user.
    /// </summary>
    public string? RecommendedAction { get; private set; }

    /// <summary>
    /// When the analysis was performed.
    /// </summary>
    public DateTime AnalyzedAt { get; private set; }

    /// <summary>
    /// Previous stress level for comparison.
    /// </summary>
    public StressLevel? PreviousStressLevel { get; private set; }

    /// <summary>
    /// Change in stress level (-3 to +3).
    /// </summary>
    public int? StressLevelChange { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private StressAnalysisResult() { }

    public static StressAnalysisResult Create(
        Guid userId,
        Guid stressDataId,
        StressLevel detectedStressLevel,
        double confidenceScore,
        double rawStressScore,
        Dictionary<string, double> componentScores,
        string analysisMessage,
        string? recommendedAction = null,
        StressLevel? previousStressLevel = null)
    {
        int? stressLevelChange = null;
        if (previousStressLevel.HasValue)
        {
            stressLevelChange = (int)detectedStressLevel - (int)previousStressLevel.Value;
        }

        var result = new StressAnalysisResult
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            StressDataId = stressDataId,
            DetectedStressLevel = detectedStressLevel,
            ConfidenceScore = confidenceScore,
            RawStressScore = rawStressScore,
            ComponentScoresJson = JsonSerializer.Serialize(componentScores),
            AnalysisMessage = analysisMessage,
            RecommendedAction = recommendedAction,
            AnalyzedAt = DateTime.UtcNow,
            PreviousStressLevel = previousStressLevel,
            StressLevelChange = stressLevelChange,
            CreatedAt = DateTime.UtcNow
        };

        result.Raise(new StressAnalysisCompletedDomainEvent(result.Id, userId, detectedStressLevel));

        return result;
    }

    public Dictionary<string, double> GetComponentScores()
    {
        return JsonSerializer.Deserialize<Dictionary<string, double>>(ComponentScoresJson) 
            ?? new Dictionary<string, double>();
    }
}

using SharedKernel;

namespace Domain.StressDetection;

/// <summary>
/// Stores the user's baseline keyboard metrics for stress comparison.
/// </summary>
public sealed class UserStressBaseline : Entity
{
    public Guid Id { get; private set; }

    /// <summary>
    /// The user this baseline belongs to.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Baseline typing speed in WPM.
    /// </summary>
    public double BaselineTypingSpeedWpm { get; private set; }

    /// <summary>
    /// Baseline error rate percentage.
    /// </summary>
    public double BaselineErrorRate { get; private set; }

    /// <summary>
    /// Baseline average key interval in milliseconds.
    /// </summary>
    public double BaselineAverageKeyInterval { get; private set; }

    /// <summary>
    /// Baseline average key press duration in milliseconds.
    /// </summary>
    public double BaselineAverageKeyPressDuration { get; private set; }

    /// <summary>
    /// Baseline correction frequency.
    /// </summary>
    public double BaselineCorrectionFrequency { get; private set; }

    /// <summary>
    /// Number of samples used to establish this baseline.
    /// </summary>
    public int SampleCount { get; private set; }

    /// <summary>
    /// When this baseline was established.
    /// </summary>
    public DateTime EstablishedAt { get; private set; }

    /// <summary>
    /// When this baseline was last updated.
    /// </summary>
    public DateTime? LastUpdatedAt { get; private set; }

    /// <summary>
    /// Whether this baseline is active and should be used for analysis.
    /// </summary>
    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private UserStressBaseline() { }

    public static UserStressBaseline Create(
        Guid userId,
        double baselineTypingSpeedWpm,
        double baselineErrorRate,
        double baselineAverageKeyInterval,
        double baselineAverageKeyPressDuration,
        double baselineCorrectionFrequency,
        int sampleCount)
    {
        return new UserStressBaseline
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            BaselineTypingSpeedWpm = baselineTypingSpeedWpm,
            BaselineErrorRate = baselineErrorRate,
            BaselineAverageKeyInterval = baselineAverageKeyInterval,
            BaselineAverageKeyPressDuration = baselineAverageKeyPressDuration,
            BaselineCorrectionFrequency = baselineCorrectionFrequency,
            SampleCount = sampleCount,
            EstablishedAt = DateTime.UtcNow,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateBaseline(
        double baselineTypingSpeedWpm,
        double baselineErrorRate,
        double baselineAverageKeyInterval,
        double baselineAverageKeyPressDuration,
        double baselineCorrectionFrequency,
        int sampleCount)
    {
        BaselineTypingSpeedWpm = baselineTypingSpeedWpm;
        BaselineErrorRate = baselineErrorRate;
        BaselineAverageKeyInterval = baselineAverageKeyInterval;
        BaselineAverageKeyPressDuration = baselineAverageKeyPressDuration;
        BaselineCorrectionFrequency = baselineCorrectionFrequency;
        SampleCount = sampleCount;
        LastUpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}

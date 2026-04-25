namespace Domain.StressDetection;

/// <summary>
/// Represents the detected stress level of a user.
/// </summary>
public enum StressLevel
{
    /// <summary>
    /// Low stress - normal typing patterns.
    /// </summary>
    Low = 1,

    /// <summary>
    /// Moderate stress - some deviation from baseline.
    /// </summary>
    Moderate = 2,

    /// <summary>
    /// High stress - significant deviation from baseline.
    /// </summary>
    High = 3,

    /// <summary>
    /// Critical stress - severe deviation, immediate intervention recommended.
    /// </summary>
    Critical = 4
}

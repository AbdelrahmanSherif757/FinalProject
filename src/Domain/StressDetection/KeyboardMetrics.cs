namespace Domain.StressDetection;

/// <summary>
/// Value object containing raw keyboard metrics from the desktop agent.
/// </summary>
public sealed class KeyboardMetrics
{
    /// <summary>
    /// Typing speed in words per minute.
    /// </summary>
    public double TypingSpeedWpm { get; set; }

    /// <summary>
    /// Variance in typing speed (standard deviation).
    /// </summary>
    public double TypingSpeedVariance { get; set; }

    /// <summary>
    /// Error rate as percentage (0-100).
    /// </summary>
    public double ErrorRate { get; set; }

    /// <summary>
    /// Average time between key presses in milliseconds.
    /// </summary>
    public double AverageKeyInterval { get; set; }

    /// <summary>
    /// Variance in key intervals (standard deviation).
    /// </summary>
    public double KeyIntervalVariance { get; set; }

    /// <summary>
    /// Average duration of a key press in milliseconds.
    /// </summary>
    public double AverageKeyPressDuration { get; set; }

    /// <summary>
    /// Number of corrections (backspace/delete) in the session.
    /// </summary>
    public int CorrectionCount { get; set; }

    /// <summary>
    /// Frequency of pauses longer than 500ms.
    /// </summary>
    public int PauseFrequency { get; set; }

    /// <summary>
    /// Total duration of the typing session in seconds.
    /// </summary>
    public int DurationSeconds { get; set; }

    /// <summary>
    /// Total characters typed in the session.
    /// </summary>
    public int CharacterCount { get; set; }

    private KeyboardMetrics() { }

    public static KeyboardMetrics Create(
        double typingSpeedWpm,
        double typingSpeedVariance,
        double errorRate,
        double averageKeyInterval,
        double keyIntervalVariance,
        double averageKeyPressDuration,
        int correctionCount,
        int pauseFrequency,
        int durationSeconds,
        int characterCount)
    {
        return new KeyboardMetrics
        {
            TypingSpeedWpm = typingSpeedWpm,
            TypingSpeedVariance = typingSpeedVariance,
            ErrorRate = errorRate,
            AverageKeyInterval = averageKeyInterval,
            KeyIntervalVariance = keyIntervalVariance,
            AverageKeyPressDuration = averageKeyPressDuration,
            CorrectionCount = correctionCount,
            PauseFrequency = pauseFrequency,
            DurationSeconds = durationSeconds,
            CharacterCount = characterCount
        };
    }
}

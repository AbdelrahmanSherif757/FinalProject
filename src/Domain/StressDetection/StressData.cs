using SharedKernel;

namespace Domain.StressDetection;

/// <summary>
/// Represents raw keyboard data submitted by the desktop agent.
/// </summary>
public sealed class StressData : Entity
{
    public Guid Id { get; private set; }

    /// <summary>
    /// The user this stress data belongs to.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// The keyboard metrics collected.
    /// </summary>
    public KeyboardMetrics Metrics { get; private set; } = null!;

    /// <summary>
    /// When the metrics were collected by the agent.
    /// </summary>
    public DateTime CollectedAt { get; private set; }

    /// <summary>
    /// When the data was submitted to the backend.
    /// </summary>
    public DateTime SubmittedAt { get; private set; }

    /// <summary>
    /// Device identifier for tracking multiple devices.
    /// </summary>
    public string? DeviceIdentifier { get; private set; }

    /// <summary>
    /// Application context (IDE, Email, Chat, etc.)
    /// </summary>
    public string? ApplicationContext { get; private set; }

    /// <summary>
    /// Whether this data has been processed for stress analysis.
    /// </summary>
    public bool IsProcessed { get; private set; }

    /// <summary>
    /// When this data was processed.
    /// </summary>
    public DateTime? ProcessedAt { get; private set; }

    /// <summary>
    /// Additional notes or context.
    /// </summary>
    public string? Notes { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private StressData() { }

    public static StressData Create(
        Guid userId,
        KeyboardMetrics metrics,
        DateTime collectedAt,
        string? deviceIdentifier = null,
        string? applicationContext = null,
        string? notes = null)
    {
        var stressData = new StressData
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Metrics = metrics,
            CollectedAt = collectedAt,
            SubmittedAt = DateTime.UtcNow,
            DeviceIdentifier = deviceIdentifier,
            ApplicationContext = applicationContext,
            IsProcessed = false,
            Notes = notes,
            CreatedAt = DateTime.UtcNow
        };

        stressData.Raise(new StressDataSubmittedDomainEvent(stressData.Id, userId));

        return stressData;
    }

    public void MarkAsProcessed()
    {
        IsProcessed = true;
        ProcessedAt = DateTime.UtcNow;
    }
}

using SharedKernel;

namespace Domain.StressDetection;

/// <summary>
/// Domain event raised when stress analysis completes.
/// </summary>
public sealed record StressAnalysisCompletedDomainEvent(
    Guid AnalysisResultId,
    Guid UserId,
    StressLevel DetectedStressLevel) : IDomainEvent;

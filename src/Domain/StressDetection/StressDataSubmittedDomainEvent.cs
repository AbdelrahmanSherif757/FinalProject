using SharedKernel;

namespace Domain.StressDetection;

/// <summary>
/// Domain event raised when stress keyboard data is submitted.
/// </summary>
public sealed record StressDataSubmittedDomainEvent(Guid StressDataId, Guid UserId) : IDomainEvent;

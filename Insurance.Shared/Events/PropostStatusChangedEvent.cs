namespace Insurance.Shared.Events;

public record PropostStatusChangedEvent(Guid PropostId, string CustomerName, decimal CoverageAmount, string Status, DateTime UpdateAt);

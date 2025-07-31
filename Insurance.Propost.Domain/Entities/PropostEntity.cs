using Insurance.Propost.Domain.ValueObjects;
using Insurance.Shared.Enums;

namespace Insurance.Propost.Domain.Entities;

public sealed class PropostEntity(string customerName, Email email, decimal coverageAmount, string insuranceType)
{
    public Guid Id { get; init; }
    public string CustomerName { get; private set; } = customerName;
    public Email Email { get; private set; } = email;
    public decimal CoverageAmount { get; private set; } = coverageAmount;
    public string InsuranceType { get; private set; } = insuranceType;
    public PropostStatus Status { get; private set; } = PropostStatus.EmAnalise;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }

    public void Approve()
    {
        if (Status != PropostStatus.EmAnalise)
        {
            throw new InvalidOperationException("Only pending proposals can be approved.");
        }
        Status = PropostStatus.Aprovada;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool CanBeContracted()
    => Status == PropostStatus.Aprovada;

    public void ChangeStatus(PropostStatus newStatus)
    => Status = newStatus;

    public void UpdateAt(DateTime dateTime)
    => UpdatedAt = dateTime;
}

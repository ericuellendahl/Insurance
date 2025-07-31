using Insurance.Shared.Enums;

namespace Insurance.Hiring.Domain.DTOs;

public record PropostDto(
Guid Id,
string CustomerName,
string Email,
decimal CoverageAmount,
string InsuranceType,
string Status,
DateTime CreatedAt,
DateTime? UpdatedAt)
{
    public bool IsApproved() => Status.Equals(PropostStatus.Aprovada.ToString(), StringComparison.CurrentCultureIgnoreCase);
}

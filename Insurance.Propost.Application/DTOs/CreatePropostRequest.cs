namespace Insurance.Propost.Application.DTOs;

public sealed record CreatePropostRequest(
                string CustomerName,
                string Email,
                decimal CoverageAmount,
                string InsuranceType
        );

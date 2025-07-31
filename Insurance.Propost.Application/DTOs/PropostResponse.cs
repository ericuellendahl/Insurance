namespace Insurance.Propost.Application.DTOs;

public record PropostResponse(Guid Id, string CustomerName, string Email, decimal CoverageAmount, string InsuranceType, string Status,
                                DateTime CreatedAt,
                                DateTime? UpdatedAt
                              );

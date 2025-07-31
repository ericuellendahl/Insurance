namespace Insurance.Hiring.Application.DTOs;

public record ContractResponse(
            Guid Id,
            Guid PropostId,
            string CustomerName,
            decimal CoverageAmount,
            DateTime ContractDate);

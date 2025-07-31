using Insurance.Hiring.Application.DTOs;
using Insurance.Hiring.Domain.DTOs;

namespace Insurance.Hiring.Application.Interfaces;

public interface IContractService
{
    Task<ContractResponse> CreateContractAsync(ContractPropostRequest request);
    Task<PropostDto?> GetContractByIdAsync(Guid id);
}

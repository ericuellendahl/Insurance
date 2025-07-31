using Insurance.Hiring.Application.DTOs;
using Insurance.Hiring.Application.Interfaces;
using Insurance.Hiring.Application.UseCase;
using Insurance.Hiring.Domain.DTOs;

namespace Insurance.Hiring.Application.Services
{
    public class ContractService(CreateContractUseCase _contractPropostUseCase, GetContractPropostUseCase _getContractPropostUseCase) : IContractService
    {
        public async Task<ContractResponse> CreateContractAsync(ContractPropostRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request cannot be null");
            }
            return await _contractPropostUseCase.ExecuteAsync(request);
        }

        public async Task<PropostDto?> GetContractByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Invalid contract ID", nameof(id));
            }
            return await _getContractPropostUseCase.ExecuteAsync(id);
        }
    }
}

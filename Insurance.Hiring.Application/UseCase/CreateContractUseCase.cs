using Insurance.Hiring.Application.DTOs;
using Insurance.Hiring.Application.Mapper;
using Insurance.Hiring.Domain.Domain;
using Insurance.Hiring.Domain.Ports;

namespace Insurance.Hiring.Application.UseCase;

public class CreateContractUseCase(IContractRepository _repository, IPropostServiceClient _propostClient)
{
    public async Task<ContractResponse> ExecuteAsync(ContractPropostRequest request)
    {
        var existingContract = await _repository.GetByProposalIdAsync(request.PropostId);
        if (existingContract is not null)
        {
            throw new InvalidOperationException("This proposal has already been contracted");
        }
        var proposal = await _propostClient.GetPropostAsync(request.PropostId) ?? throw new InvalidOperationException("Propost not found");

        if (!proposal.IsApproved())
            throw new InvalidOperationException("Only approved proposals can be contracted");

        var contract = new ContractEntity(
            proposal.Id,
            proposal.CustomerName,
            proposal.CoverageAmount
        );

        await _repository.CreateAsync(contract);

        return contract.ToResponse();
    }
}

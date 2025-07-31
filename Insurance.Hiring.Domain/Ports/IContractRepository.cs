using Insurance.Hiring.Domain.Domain;

namespace Insurance.Hiring.Domain.Ports;

public interface IContractRepository
{
    Task<ContractEntity> CreateAsync(ContractEntity contract);
    Task<ContractEntity?> GetByIdAsync(Guid id);
    Task<ContractEntity?> GetByProposalIdAsync(Guid propostId);
    Task<IEnumerable<ContractEntity>> GetAllAsync();
    Task UpdateAsync(ContractEntity contract);
}

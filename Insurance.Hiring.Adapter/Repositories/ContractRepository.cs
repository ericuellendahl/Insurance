using Insurance.Hiring.Domain.Domain;
using Insurance.Hiring.Domain.Ports;
using Insurance.Hiring.Infra.Data;
using Microsoft.EntityFrameworkCore;
namespace Insurance.Hiring.Adapter.Repositories;
public class ContractRepository(ContractDbContext _contractDbContext) : IContractRepository
{
    public async Task<ContractEntity> CreateAsync(ContractEntity contract)
    {
        await _contractDbContext.Contracts.AddAsync(contract);
        await _contractDbContext.SaveChangesAsync();

        return contract;
    }

    public async Task<IEnumerable<ContractEntity>> GetAllAsync()
    => await _contractDbContext.Contracts
            .AsNoTracking()
            .OrderByDescending(c => c.ContractDate)
            .ToListAsync();


    public async Task<ContractEntity?> GetByIdAsync(Guid id)
    => await _contractDbContext.Contracts
            .FirstOrDefaultAsync(c => c.Id == id);


    public async Task<ContractEntity?> GetByProposalIdAsync(Guid propostId)
    => await _contractDbContext.Contracts
            .FirstOrDefaultAsync(c => c.PropostId == propostId);

    public async Task UpdateAsync(ContractEntity contract)
    {
        _contractDbContext.Contracts.Update(contract);
        await _contractDbContext.SaveChangesAsync();
    }
}

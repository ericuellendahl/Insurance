using Insurance.Propost.Domain.Entities;

namespace Insurance.Propost.Domain.Ports;

public interface IPropostRepository
{
    Task<PropostEntity> CreateAsync(PropostEntity propostEntity);
    Task<PropostEntity?> GetByIdAsync(Guid id);
    Task<IEnumerable<PropostEntity>> GetAllAsync();
    Task<PropostEntity> UpdateAsync(PropostEntity propostEntity);
}

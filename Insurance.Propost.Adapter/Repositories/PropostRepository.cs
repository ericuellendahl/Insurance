using Insurance.Propost.Domain.Entities;
using Insurance.Propost.Domain.Ports;
using Insurance.Propost.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Propost.Adapter.Repositories;

public class PropostRepository(PropostDbContext _propostDbContext) : IPropostRepository
{
    public async Task<PropostEntity> CreateAsync(PropostEntity propostEntity)
    {
        await _propostDbContext.Proposts.AddAsync(propostEntity);
        await _propostDbContext.SaveChangesAsync();
        return propostEntity;
    }

    public async Task<IEnumerable<PropostEntity>> GetAllAsync()
    {
        return await _propostDbContext.Proposts
                                      .AsNoTracking()
                                      .OrderByDescending(p => p.CreatedAt)
                                      .ToListAsync();
    }

    public async Task<PropostEntity?> GetByIdAsync(Guid id)
    {
        return await _propostDbContext.Proposts.FindAsync(id);
    }

    public async Task<PropostEntity> UpdateAsync(PropostEntity propostEntity)
    {
        _propostDbContext.Proposts.Update(propostEntity);
        await _propostDbContext.SaveChangesAsync();
        return propostEntity;
    }
}

using Insurance.Propost.Application.DTOs;
using Insurance.Propost.Application.Interfaces;
using Insurance.Propost.Application.Mapper;
using Insurance.Propost.Application.UseCase;
using Insurance.Propost.Domain.Ports;

namespace Insurance.Propost.Application.Services;

public class PropostService(CreatePropostUseCase _createPropostUseCase,
                            ChangePropostStatusUseCase _changePropostStatusUseCase,
                            IPropostRepository _repository) : IPropostService
{
    public async Task<PropostResponse> CreatePropostAsync(CreatePropostRequest request)
    {
        return await _createPropostUseCase.ExecuteAsync(request);
    }
    public async Task<PropostResponse> ChangePropostStatusAsync(ChangePropostStatusRequest request)
    {
        return await _changePropostStatusUseCase.ExecuteAsync(request);
    }
    public async Task<IEnumerable<PropostResponse>> GetAllPropostsAsync()
    {
        var proposts = await _repository.GetAllAsync();
        return proposts.Select(p => p.MapToResponse());
    }
    public async Task<PropostResponse> GetPropostaAsync(Guid id)
    {
        var propost = await _repository.GetByIdAsync(id);
        return propost is null
            ? throw new KeyNotFoundException("Propost not found")
            : propost.MapToResponse();
    }
}

using Insurance.Propost.Application.DTOs;

namespace Insurance.Propost.Application.Interfaces;

public interface IPropostService
{
    Task<PropostResponse> ChangePropostStatusAsync(ChangePropostStatusRequest request);
    Task<PropostResponse> CreatePropostAsync(CreatePropostRequest request);
    Task<IEnumerable<PropostResponse>> GetAllPropostsAsync();
    Task<PropostResponse> GetPropostaAsync(Guid id);
}

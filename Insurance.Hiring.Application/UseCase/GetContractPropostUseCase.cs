using Insurance.Hiring.Domain.DTOs;
using Insurance.Hiring.Domain.Ports;

namespace Insurance.Hiring.Application.UseCase
{
    public class GetContractPropostUseCase(IPropostServiceClient _propostServiceClient)
    {
        public async Task<PropostDto?> ExecuteAsync(Guid propostId)
        {
            if (propostId == Guid.Empty)
            {
                throw new ArgumentException("Propost ID cannot be empty", nameof(propostId));
            }
            var propost = await _propostServiceClient.GetPropostAsync(propostId);

            return propost ?? throw new InvalidOperationException("Proposal not found");
        }
    }
}

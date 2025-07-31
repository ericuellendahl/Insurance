using Insurance.Hiring.Domain.DTOs;

namespace Insurance.Hiring.Domain.Ports;

public interface IPropostServiceClient
{
    Task<PropostDto?> GetPropostAsync(Guid propostId);
}
using Flurl.Http;
using Insurance.Hiring.Domain.DTOs;
using Insurance.Hiring.Domain.Ports;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Insurance.Hiring.Adapter.ExternalServices;

public class PropostServiceClient(IConfiguration configuration, ILogger<PropostServiceClient> _logger) : IPropostServiceClient
{
    private readonly string _baseUrl = configuration["Services:PropostService:BaseUrl"] ??
                   throw new InvalidOperationException("ProposalService URL not configured");

    public async Task<PropostDto?> GetPropostAsync(Guid propostId)
    {
        try
        {
            var response = await $"{_baseUrl}/api/propost/{propostId}".GetJsonAsync<PropostDto>();

            if (response is null)
            {
                _logger.LogWarning("Proposal with ID {PropostId} not found", propostId);
                return null;
            }

            _logger.LogInformation("Successfully retrieved proposal with ID {PropostId}", propostId);

            return response;
        }
        catch (FlurlHttpException ex) when (ex.Call.Response?.StatusCode == 404)
        {
            throw new InvalidOperationException($"Proposal with ID {propostId} not found", ex);
        }
    }
}

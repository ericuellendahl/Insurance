using Insurance.Hiring.Application.DTOs;
using Insurance.Hiring.Domain.Domain;

namespace Insurance.Hiring.Application.Mapper
{
    public static class MappeToResponse
    {
        public static ContractResponse ToResponse(this ContractEntity contract)
        {
            return new ContractResponse(
                contract.Id,
                contract.PropostId,
                contract.CustomerName,
                contract.CoverageAmount,
                contract.ContractDate
            );
        }
    }
}

using Insurance.Propost.Application.DTOs;
using Insurance.Propost.Domain.Entities;
using Insurance.Shared.Enums;

namespace Insurance.Propost.Application.Mapper
{
    public static class MappeToResponse
    {
        public static PropostResponse MapToResponse(this PropostEntity entity)
        {
            return new PropostResponse(
                entity.Id,
                entity.CustomerName,
                entity.Email,
                entity.CoverageAmount,
                entity.InsuranceType,
                Enum.GetName(typeof(PropostStatus), entity.Status) ?? "Unknown",
                entity.CreatedAt,
                entity.UpdatedAt
            );
        }
    }
}

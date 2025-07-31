using Insurance.Propost.Application.DTOs;
using Insurance.Propost.Application.Mapper;
using Insurance.Propost.Domain.Entities;
using Insurance.Propost.Domain.Ports;
using Insurance.Propost.Domain.ValueObjects;

namespace Insurance.Propost.Application.UseCase;

public class CreatePropostUseCase(IPropostRepository _repository)
{
    public async Task<PropostResponse> ExecuteAsync(CreatePropostRequest request)
    {
        var emailVo = new Email(request.Email);

        var propost = new PropostEntity(
            request.CustomerName,
            emailVo,
            request.CoverageAmount,
            request.InsuranceType
        );

        await _repository.CreateAsync(propost);

        return propost.MapToResponse();
    }
}

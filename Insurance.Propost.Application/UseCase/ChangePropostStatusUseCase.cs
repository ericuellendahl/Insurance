using Insurance.Propost.Application.DTOs;
using Insurance.Propost.Application.Mapper;
using Insurance.Shared.Enums;
using Insurance.Propost.Domain.Ports;
using Insurance.Shared.Events;

namespace Insurance.Propost.Application.UseCase;

public class ChangePropostStatusUseCase(IPropostRepository _repository, IEventPublisher _eventPublisher)
{
    public async Task<PropostResponse> ExecuteAsync(ChangePropostStatusRequest request)
    {
        var propost = await _repository.GetByIdAsync(request.PropostId) ?? throw new InvalidOperationException("Proposal not found");

        propost.ChangeStatus(request.NewStatus);
        propost.UpdateAt(DateTime.UtcNow);

        await _repository.UpdateAsync(propost);

        if (propost.Status == PropostStatus.Aprovada)
        {
            var @event = new PropostStatusChangedEvent(
                propost.Id,
                propost.CustomerName,
                propost.CoverageAmount,
                PropostStatus.Aprovada.ToString(),
                DateTime.UtcNow
            );

            await _eventPublisher.PublishAsync(@event);
        }

        return propost.MapToResponse();
    }
}

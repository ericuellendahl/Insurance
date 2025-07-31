using Insurance.Hiring.Domain.Domain;
using Insurance.Hiring.Domain.Ports;
using Insurance.Shared.Enums;
using Insurance.Shared.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Insurance.Hiring.Adapter.Consumer
{
    public class PropostStatusChangedConsumer(ILogger<PropostStatusChangedConsumer> _logger, IContractRepository _repository) : IConsumer<PropostStatusChangedEvent>
    {

        public async Task Consume(ConsumeContext<PropostStatusChangedEvent> context)
        {
            var @event = context.Message;

            _logger.LogInformation("Propost {PropostId} had its status changed to {NewStatus} on {ChangeDate}",
                @event.PropostId, @event.Status, @event.UpdateAt);

            if (@event.Status.ToLower() == PropostStatus.Aprovada.ToString().ToLower())
            {
                var contract = await _repository.GetByProposalIdAsync(@event.PropostId);
                if (contract is null)
                {
                    _logger.LogInformation("Creating new contract for Propost {ProposalId}", @event.PropostId);
                    contract = new ContractEntity(@event.PropostId, @event.CustomerName, @event.CoverageAmount);
                    await _repository.CreateAsync(contract);
                }
                else
                {
                    _logger.LogInformation("Propost {ProposalId} already has a contract {ContractId}",
                         @event.PropostId, contract.Id);

                    contract = new ContractEntity(@event.PropostId, @event.CustomerName, @event.CoverageAmount);

                    await _repository.UpdateAsync(contract);
                }
            }

            await Task.CompletedTask;
        }
    }
}

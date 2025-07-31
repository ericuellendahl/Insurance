using Insurance.Propost.Domain.Ports;
using MassTransit;

namespace Insurance.Propost.Adapter.EventPublisher;

public class MassTransitEventPublisher(IPublishEndpoint _publishEndpoint) : IEventPublisher
{
    public async Task PublishAsync<T>(T @event) where T : class
    {
        await _publishEndpoint.Publish(@event);
    }
}

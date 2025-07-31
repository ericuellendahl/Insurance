namespace Insurance.Propost.Domain.Ports
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T @event) where T : class;
    }
}

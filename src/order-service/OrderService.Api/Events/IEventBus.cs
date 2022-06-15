namespace OrderService.Api.Events;

public interface IEventBus
{
    Task PublishAsync(IEvent evt);
}
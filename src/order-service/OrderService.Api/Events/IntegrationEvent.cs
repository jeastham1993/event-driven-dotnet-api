namespace OrderService.Api.Events;

public abstract class IntegrationEvent
{
    public abstract string EventName { get; }
    public string Source => "order-service";
    
    public IEvent? Data { get; }
}
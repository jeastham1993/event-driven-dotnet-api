namespace OrderService.Api.Events;

public interface IEvent
{
    string EventName { get; }

    string Source {get;}
}
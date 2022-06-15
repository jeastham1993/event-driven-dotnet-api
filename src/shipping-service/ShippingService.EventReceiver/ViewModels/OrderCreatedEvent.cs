namespace OrderService.Api.Events;

public class EventWrapper
{
    public EventWrapper()
    {
    }

    public string version { get; set; } = string.Empty;
    public string source { get; set; } = string.Empty;
    public OrderCreatedEventData detail { get; set; }
}

public class OrderCreatedEventData
{
    public OrderCreatedEventData()
    {
        this.CustomerId = string.Empty;
        this.OrderNumber = string.Empty;
    }

    public string CustomerId { get; set; }

    public string OrderNumber { get; set; }
}

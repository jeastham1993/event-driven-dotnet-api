using OrderService.Api.Models;

namespace OrderService.Api.Events;

public class OrderCreatedEvent : IEvent
{
    public OrderCreatedEvent(Order order)
    {
        this.Data = new OrderCreatedEventData()
        {
            CustomerId = order.CustomerId,
            OrderNumber = order.OrderNumber
        };
    }
    public string EventName => "order-service.order-created";

    public string Source => "order-service";

    public OrderCreatedEventData Data { get; set; }
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

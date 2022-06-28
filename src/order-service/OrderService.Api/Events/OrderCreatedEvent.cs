using OrderService.Api.Models;

namespace OrderService.Api.Events;

public class OrderCreatedEvent : IntegrationEvent
{
    public OrderCreatedEvent(Order order)
    {
        this.Data = new OrderCreatedEventData()
        {
            CustomerId = order.CustomerId,
            OrderNumber = order.OrderNumber
        };
    }
    public override string EventName => "order-service.order-created";
    
    public new OrderCreatedEventData Data { get;  }
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

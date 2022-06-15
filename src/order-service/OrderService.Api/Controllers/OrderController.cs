using Microsoft.AspNetCore.Mvc;
using OrderService.Api.Events;
using OrderService.Api.Models;

namespace OrderService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;
    private readonly IOrderRepository _orderRepo;

    private readonly IEventBus _eventBus;

    public OrderController(ILogger<OrderController> logger, IOrderRepository orderRepo, IEventBus eventBus)
    {
        this._logger = logger;
        this._orderRepo = orderRepo;
        this._eventBus = eventBus;
    }

    [HttpPost(Name = "CreateOrder")]
    public async Task<IActionResult> Get([FromBody] CreateOrderCommand command)
    {
        if (command.CustomerId is null)
        {
            return this.BadRequest();
        }

        var order = new Order(command.CustomerId);

        await this._orderRepo.CreateOrder(order);

        await this._eventBus.PublishAsync(new OrderCreatedEvent(order));

        return this.Ok(order);
    }
}

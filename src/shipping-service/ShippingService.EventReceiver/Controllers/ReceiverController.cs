using Microsoft.AspNetCore.Mvc;
using OrderService.Api.Events;
using ShippingService.EventReceiver.Models;
using System.Text.Json;

namespace ShippingService.EventReceiver.Controllers;

[ApiController]
[Route("[controller]")]
public class ReceiverController : ControllerBase
{
    private readonly ILogger<ReceiverController> _logger;
    private readonly IShippingRepository _shippingRepo;

    public ReceiverController(ILogger<ReceiverController> logger, IShippingRepository shippingRepo)
    {
        _logger = logger;
        this._shippingRepo = shippingRepo;
    }

    [HttpPost(Name = "ReceiveEvent")]
    public async Task<IActionResult> ReceiveEvent([FromBody] EventWrapper evtData)
    {
        this._logger.LogInformation(JsonSerializer.Serialize(evtData));
        await this._shippingRepo.StoreAsync(new ShippingData(evtData.detail.Data.CustomerId, evtData.detail.Data.OrderNumber));
        
        return this.Ok();
    }
}

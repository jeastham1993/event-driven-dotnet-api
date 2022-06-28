using System.Text.Json;
using Amazon.EventBridge;
using Amazon.EventBridge.Model;
using Newtonsoft.Json;
using OrderService.Api.Events;

namespace OrderService.Api.Infrastructure;

public class EventBridgeEventBus : IEventBus
{
    public AmazonEventBridgeClient _eventBridgeClient;

    public EventBridgeEventBus(AmazonEventBridgeClient eventBridgeClient)
    {
        this._eventBridgeClient = eventBridgeClient;
    }

    public async Task PublishAsync(IntegrationEvent evt)
    {
        await this._eventBridgeClient.PutEventsAsync(new PutEventsRequest()
        {
            Entries = new List<PutEventsRequestEntry>(1)
            {
                new PutEventsRequestEntry()
                {
                    Detail = JsonConvert.SerializeObject(evt),
                    DetailType = evt.EventName,
                    Source = evt.Source,
                    EventBusName = Environment.GetEnvironmentVariable("EVENT_BUS_NAME"),
                }
            }
        });
    }
}

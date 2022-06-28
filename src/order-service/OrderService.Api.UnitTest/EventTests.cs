using Newtonsoft.Json;
using OrderService.Api.Events;
using OrderService.Api.Models;

namespace OrderService.Api.UnitTest;

public class EventTests
{
    [Fact]
    public void TestSerialization_ShouldSerializeAllProperties()
    {
        var evt = new OrderCreatedEvent(new Order("jameseastham"));

        var result = JsonConvert.SerializeObject(evt);
        
        Assert.True(result.Contains("\"CustomerId\":\"jameseastham\""));
    }
}
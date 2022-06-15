using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using ShippingService.EventReceiver.Models;

namespace ShippingService.EventReceiver.Infrastructure;
public class ShippingRepository : IShippingRepository
{
    private readonly AmazonDynamoDBClient _dynamoClient;

    public ShippingRepository(AmazonDynamoDBClient dynamoClient)
    {
        this._dynamoClient = dynamoClient;
    }

    public async Task StoreAsync(ShippingData data)
    {
        await this._dynamoClient.PutItemAsync(Environment.GetEnvironmentVariable("TABLE_NAME"), new Dictionary<string, Amazon.DynamoDBv2.Model.AttributeValue>(2)
        {
            {"PK", new AttributeValue(data.CustomerId.ToUpper())},
            {"SK", new AttributeValue(data.OrderNumber.ToUpper())},
        });
    }
}

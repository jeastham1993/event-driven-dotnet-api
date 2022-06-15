using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using OrderService.Api.Models;

namespace OrderService.Api.Infrastructure;

public class OrderRepository : IOrderRepository
{
    private readonly AmazonDynamoDBClient _dynamoClient;

    public OrderRepository(AmazonDynamoDBClient dynamoClient)
    {
        this._dynamoClient = dynamoClient;
    }

    public async Task CreateOrder(Order order)
    {
        await this._dynamoClient.PutItemAsync(Environment.GetEnvironmentVariable("TABLE_NAME"), new Dictionary<string, Amazon.DynamoDBv2.Model.AttributeValue>(2)
        {
            {"PK", new AttributeValue(order.CustomerId.ToUpper())},
            {"SK", new AttributeValue(order.OrderNumber.ToUpper())},
        });
    }
}

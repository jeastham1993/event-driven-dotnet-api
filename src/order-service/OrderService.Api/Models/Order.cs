namespace OrderService.Api.Models;

public class Order
{
    public Order(){
        this.CustomerId = string.Empty;
        this.OrderNumber = string.Empty;
    }

    public Order(string customerId)
    {
        this.CustomerId = customerId;
        this.OrderNumber = Guid.NewGuid().ToString();
    }

    public string CustomerId { get; set; }

    public string OrderNumber { get; set; }
}
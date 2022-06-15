namespace ShippingService.EventReceiver.Models;

public class ShippingData 
{
    public ShippingData(string customerId, string orderNumber)
    {
        this.CustomerId = customerId;
        this.OrderNumber = orderNumber;
    }
    
    public string CustomerId {get;set;}

    public string OrderNumber {get;set;}
}
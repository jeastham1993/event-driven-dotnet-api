namespace ShippingService.EventReceiver.Models;

public interface IShippingRepository {
    Task StoreAsync(ShippingData data);
}
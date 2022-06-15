
namespace OrderService.Api.Models;

public interface IOrderRepository
{
    Task CreateOrder(Order order);
}
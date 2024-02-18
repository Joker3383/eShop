using Order.API.Models.Dto;

namespace Order.API.Services.Interfaces;

public interface IOrderService
{
    Task<OrderDto> CreateOrder(int subId);
    Task<IEnumerable<OrderDto>> GetOrders(int subId);
    Task<OrderDto> DeleteOrder(int Id);
}
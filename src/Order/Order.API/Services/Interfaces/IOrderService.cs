using Order.API.Models.Dto;

namespace Order.API.Services.Interfaces;

public interface IOrderService
{
    Task<OrderDto> CreateOrder(string login);
    Task<IEnumerable<OrderDto>> GetOrders(string login);
}
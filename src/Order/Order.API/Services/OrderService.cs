using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Order.API.Models.Dto;
using Order.API.Repositories.Interfaces;
using Order.API.Services.Interfaces;

namespace Order.API.Services;

public class OrderService : IOrderService
{
    private IMapper _mapper;
    private IBasketRepository _basketRepository;
    private IOrderRepository _orderRepository;

    public OrderService(IBasketRepository basketRepository, IMapper mapper, IOrderRepository orderRepository)
    {
        _mapper = mapper;
        _basketRepository = basketRepository;
        _orderRepository = orderRepository;
    }
    public async Task<OrderDto> CreateOrder(string login)
    {
        var allShoppingCarts = await _basketRepository.GetShoppingCarts(login);

        double sum = 0;
        foreach (var shoppingCart in allShoppingCarts)
        {
           var totalSum = shoppingCart.Product;
           sum += totalSum.Price;

        }

        var order = new Models.Order
        {
            Login = login,
            ShoppingCarts = allShoppingCarts,
            DateOfOrder = DateTime.Now,
            TotalSum = sum,

        };
        var addedOrder = await _orderRepository.CreateOrder(order);

        return _mapper.Map<OrderDto>(addedOrder);
    }

    public async Task<IEnumerable<OrderDto>> GetOrders(string login)
    {
        var orders = _orderRepository.FindAll().AsNoTracking();

        var filteredOrders = orders.Where(o => o.Login == login);
       var ppp = _mapper.Map<IEnumerable<OrderDto>>(filteredOrders.ToList());
       return ppp;



    }
}
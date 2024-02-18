using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.API.Data;
using Order.API.Models.Dto;
using Order.API.Repositories.Interfaces;
using Order.API.Services.Interfaces;
using Shared.CrudOperations;

namespace Order.API.Services;

public class OrderService : IOrderService
{
    private readonly IMapper _mapper;
    private readonly IBasketRepository _basketRepository;
    private readonly IMediator _mediator;

    public OrderService(IBasketRepository basketRepository, IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _basketRepository = basketRepository;
        _mediator = mediator;
    }
    public async Task<OrderDto> CreateOrder(int subId)
    {
        var basket = await _basketRepository.GetBasketsAsync(subId);

        if (basket == null)
        {
            throw new NullReferenceException($"By id: {subId} no basket");
        }
        
        var order = new Models.Order
        {
            SubId = subId,
            BasketId = basket.Id,
            DateOfOrder = DateTime.Now,
            TotalSum = basket.TotalCount

        };
        await _mediator.Send(new CreateEntityCommand<Models.Order, AppDbContext>(order));

        var mapped = _mapper.Map<OrderDto>(order);
        return mapped;
    }

    public async Task<IEnumerable<OrderDto>> GetOrders(int subId)
    {
        var orders = await _mediator.Send(new GetEntitiesBySubIdQuery<Models.Order, AppDbContext>(subId));
        if (orders == null)
        {
            throw new MediatorException("Orders are empty");
        }
        
        return _mapper.Map<IEnumerable<OrderDto>>(orders.ToList());
        
    }
    
    public async Task<OrderDto> DeleteOrder(int id)
    {
        var order = await _mediator.Send(new GetEntityByIdQuery<Models.Order, AppDbContext>(id));
        if (order == null)
        {
            throw new MediatorException("Order is empty");
        }

        await _mediator.Send(new DeleteEntityCommand<Models.Order, AppDbContext>(order));

        return _mapper.Map<OrderDto>(order);
    }
}
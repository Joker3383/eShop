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
        var ordersBySubId = await _mediator.Send(new GetEntitiesBySubIdQuery<Models.Order, AppDbContext>(subId));
        if (ordersBySubId == null)
        {
            throw new MediatorException("Orders are empty");
        }
        
        return _mapper.Map<IEnumerable<OrderDto>>(ordersBySubId.ToList());
        
    }
    public async Task DeleteOrder(int subId)
    {
        var ordersBySubId = await _mediator.Send(new GetEntitiesBySubIdQuery<Models.Order, AppDbContext>(subId));
        if (ordersBySubId == null)
        {
            throw new MediatorException("Orders are empty");
        }
        await _mediator.Send(new DeleteAllCommand<Models.Order, AppDbContext>(subId));
    }
}
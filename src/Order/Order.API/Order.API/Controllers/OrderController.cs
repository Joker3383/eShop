namespace Order.API.Controllers;

[Authorize(Policy = "AuthenteficatedUser")]
[ApiController]
[Route("/api/order")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private  readonly ResponseDto _response;
    
    
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
        _response = new ResponseDto();
    }

    [HttpGet("/{subId}")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    public async  Task<ResponseDto> GetOrders(int subId)
    {
        
        try
        {
            var orders = await _orderService.GetOrders(subId);
            if (orders == null)
                throw new NullReferenceException($"By Login:{subId} there aren`t orders");
            _response.Result = orders;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }

        return _response;
    }

    [HttpPost("/{subId}")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    public async Task<ResponseDto> MakeAnOrder(int subId)
    {
        try
        {
            var addedOrder = await _orderService.CreateOrder(subId);
            if (addedOrder == null)
                throw new NullReferenceException("Order is not add");
            
            _response.Result = addedOrder;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }

        return _response;
    }
    
    [HttpDelete("/{Id}")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    public async Task<ResponseDto> DeleteAnOrder(int Id)
    {
        try
        {
            await _orderService.DeleteOrder(Id);
            _response.Message = "You've deleted orders!";
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }

        return _response;
    }


    
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.API.Models.Dto;
using Order.API.Services.Interfaces;

namespace Order.API.Controllers;

[Authorize(Policy = "AuthenteficatedUser")]
[ApiController]
[Route("/api/order")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private  ResponseDto _response;
    
    
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
        _response = new ResponseDto();
    }

    [HttpGet("/{login}")]
    public async  Task<ResponseDto> GetOrders(string login)
    {
        
        try
        {
            var ordersByLogin = await _orderService.GetOrders(login);
            if (ordersByLogin == null)
                throw new NullReferenceException($"By Login:{login} there aren`t orders");
            _response.Result = ordersByLogin;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }

        return _response;
    }

    [HttpPost("/{login}")]
    public async Task<ResponseDto> AddOrder(string login)
    {
        try
        {
            var addedOrder = await _orderService.CreateOrder(login);
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
}
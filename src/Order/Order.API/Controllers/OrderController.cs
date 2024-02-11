using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.API.Models.Dto;
using Order.API.Repositories;
using Order.API.Repositories.Interfaces;
using Order.API.Services.Interfaces;

namespace Order.API.Controllers;

[Authorize(Policy = "AuthenteficatedUser")]
[ApiController]
[Route("/api/order")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private  ResponseDto _response;
    private IOrderRepository _repository;
    
    
    public OrderController(IOrderService orderService, IOrderRepository repository)
    {
        _orderService = orderService;
        _response = new ResponseDto();
        _repository = repository;
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


    [HttpGet]

    public async Task<IActionResult> Get()
    {
        var result = await _repository.CreateOrder(new Models.Order
        {
            Id = 1,
            Login = "88421113",
            DateOfOrder = DateTime.Now,
            ShoppingCarts = new List<ShoppingCartDto>
            {
                new ShoppingCartDto
                {
                    Id = 25,
                    OrderId = 0,
                    ProductId = 1,
                    Product = new ProductDto
                    {
                        CategoryName = "Electronics",
                        Description = "A fantastic electronic device.",
                        ProductId = 1,
                        Name = "Product 1",
                        Price = 20,
                        ImageUrl = "",
                    }
                }
            }

        });

        return Ok(result);
    }
}
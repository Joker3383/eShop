using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services.Interfaces;
using Newtonsoft.Json;

namespace MVC.Controllers;

public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }
    public async Task<IActionResult> OrderIndex()
    {
        var subId = "bob";//User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        var response = await _orderService.GetOrdersAsync(subId);
        var orders = new List<OrderDto>();
        if (response != null && response.IsSuccess)
        {
            orders = JsonConvert.DeserializeObject<List<OrderDto>>(Convert.ToString(response.Result));
        }
        else
        {
            TempData["error"] = response?.Message;
        }
        return View(orders);
        
    }
    
    public async Task<IActionResult> CreateIndex()
    {
        var subId = "bob";//User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        var response = await _orderService.CreateOrderAsync(subId);
        return RedirectToAction("OrderIndex");
    }

    public async Task<IActionResult> OrderDetails(int orderId)
    {
        var subId = "bob";//User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        var response = await _orderService.GetOrdersAsync(subId);
        var orders = new List<OrderDto>();
        if (response != null && response.IsSuccess)
        {
            orders = JsonConvert.DeserializeObject<List<OrderDto>>(Convert.ToString(response.Result));
        }
        else
        {
            TempData["error"] = response?.Message;
        }

        var order = orders.FirstOrDefault(o => o.Id == orderId);
        return View(order);
    }
}
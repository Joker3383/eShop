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
        var subId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        var response = await _orderService.GetOrdersAsync(subId);
    
        if (response == null || !response.IsSuccess)
        {
            TempData["error"] = response?.Message ?? "Failed to retrieve orders.";
            return RedirectToAction("Error", "Home"); 
        }
    
        var orders = JsonConvert.DeserializeObject<List<OrderDto>>(Convert.ToString(response.Result));
        return View(orders);
    }

    
    public async Task<IActionResult> CreateOrder()
    {
        var subId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        if (subId == null)
        {
            TempData["error"] = "User not authenticated.";
            return RedirectToAction("Error", "Home"); 
        }

        var response = await _orderService.CreateOrderAsync(subId);
        if (!response.IsSuccess)
        {
            TempData["error"] = response.Message;
            return RedirectToAction("Error", "Home"); 
        }
    
        return RedirectToAction("OrderIndex");
    }

    public async Task<IActionResult> OrderDetails(int orderId)
    {
        var subId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        var response = await _orderService.GetOrdersAsync(subId);

        if (response == null || !response.IsSuccess)
        {
            TempData["error"] = response?.Message ?? "Failed to retrieve order details.";
            return RedirectToAction("Error", "Home"); 
        }
    
        var orders = JsonConvert.DeserializeObject<List<OrderDto>>(Convert.ToString(response.Result));
        return View(orders.FirstOrDefault(o => o.Id == orderId));
    }

}
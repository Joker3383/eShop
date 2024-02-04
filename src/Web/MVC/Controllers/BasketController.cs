using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services.Interfaces;
using Newtonsoft.Json;

namespace MVC.Controllers;

public class BasketController : Controller
{
    private readonly IBasketService _basketService;

    public BasketController(IBasketService basketService)
    {
        _basketService = basketService;
    }

    public async Task<IActionResult>  BasketIndex()
    {
        var subId = "1";//User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        var response = await _basketService.GetShoppingCartsAsync(subId);
        var shoppingCarts = new List<ShoppingCartDto>();
        if (response != null && response.IsSuccess)
        {
            shoppingCarts = JsonConvert.DeserializeObject<List<ShoppingCartDto>>(Convert.ToString(response.Result));
        }
        else
        {
            TempData["error"] = response?.Message;
        }
        return View(shoppingCarts.Select(cart => cart.Product).ToList());
    }

    public async Task<IActionResult> AddShoppingCartAsync(int productId)
    {
        var subId = "1";//User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        var addedShoppingCart = await _basketService.AddShoppingCartAsync(subId, productId);
        return RedirectToAction("BasketIndex");
    }

    
}
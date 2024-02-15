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
        
        var subId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        var response = await _basketService.GetBasketAsync(subId);
        if (response == null)
        {
             response = await _basketService.CreateBasketAsync(subId);
             TempData["error"] = response?.Message;
        }
        
        var basket  = JsonConvert.DeserializeObject<BasketDto>(Convert.ToString(response.Result));
       
        return View(basket);
    }

    public async Task<IActionResult> AddProductIntoBasketAsync(int productId, int quantity)
    {
        var subId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        var addedShoppingCart = await _basketService.AddProductIntoBasketAsync(subId, productId, quantity);
        return RedirectToAction("BasketIndex");
    }
    
    public async Task<IActionResult> RemoveProductFromBasketAsync(int productId, int quantity)
    {
        var subId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        var addedShoppingCart = await _basketService.DeleteProductFromBasketAsync(subId, productId, quantity);
        return RedirectToAction("BasketIndex");
    }
    
    
    
    

    
}
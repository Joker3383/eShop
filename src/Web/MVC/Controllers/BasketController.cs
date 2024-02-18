using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Controllers
{
    public class BasketController : Controller
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
        }

        public async Task<IActionResult> BasketIndex()
        {
            var subId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            var response = await _basketService.GetBasketAsync(subId);

            if (response == null || !response.IsSuccess)
            {
                TempData["error"] = response?.Message ?? "Failed to retrieve or create basket.";
                return RedirectToAction("Error", "Home"); 
            }

            var basket = JsonConvert.DeserializeObject<BasketDto>(Convert.ToString(response.Result));
            return View(basket);
        }


        public async Task<IActionResult> AddProductIntoBasket(int productId, int quantity)
        {
            var subId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            if (subId == null)
            {
                TempData["error"] = "User not authenticated.";
                return RedirectToAction("Error", "Home"); 
            }

            var result = await _basketService.AddProductIntoBasketAsync(subId, productId, quantity);
            if (!result.IsSuccess)
            {
                TempData["error"] = result.Message;
                return RedirectToAction("Error", "Home"); 
            }

            return RedirectToAction("BasketIndex");
        }


        public async Task<IActionResult> RemoveProductFromBasket(int productId, int quantity)
        {
            var subId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            if (subId == null)
            {
                TempData["error"] = "User not authenticated.";
                return RedirectToAction("Error", "Home"); 
            }

            var result = await _basketService.DeleteProductFromBasketAsync(subId, productId, quantity);
            if (!result.IsSuccess)
            {
                TempData["error"] = result.Message;
                return RedirectToAction("Error", "Home"); 
            }

            return RedirectToAction("BasketIndex");
        }
    }
}

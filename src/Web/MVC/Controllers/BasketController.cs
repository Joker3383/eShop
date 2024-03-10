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
                var result  = await _basketService.CreateBasketAsync(subId);
                if (result == null || !result.IsSuccess)
                {
                    TempData["error"] = "Basket cannot create.";
                    return RedirectToAction("Error", "Home"); 
                }
                
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
        
        public async Task<IActionResult> Delete()
        {
            var subId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            var response = await _basketService.DeleteBasketAsync(subId);
        
            if (!response.IsSuccess)
            {
                TempData["error"] = response.Message; 
                return RedirectToAction("OrderIndex", "Order"); 
            }
        
            return RedirectToAction("BasketIndex");
        }
    }
}

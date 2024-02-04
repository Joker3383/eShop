using MVC.Models;
using MVC.Services.Interfaces;

namespace MVC.Services;

public class BasketService : IBasketService
{
    private IBaseService _baseService;

    public BasketService(IBaseService baseService)
    {
        _baseService = baseService;
    }
    public async Task<ResponseDto?> GetShoppingCartsAsync(string subjectId)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            Sd = SD.ApiType.GET,
            Url = SD.BasketAPIBase + $"/{subjectId}"
        });
    }

    public async Task<ResponseDto?> AddShoppingCartAsync(string subjectId, int productId)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            Sd = SD.ApiType.POST,
            Url = SD.BasketAPIBase + $"/{subjectId}/{productId}"
        });
    }
}
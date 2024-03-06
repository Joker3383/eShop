
namespace MVC.Services;

public class BasketService : IBasketService
{
    private IBaseService _baseService;

    public BasketService(IBaseService baseService)
    {
        _baseService = baseService;
    }
    public async Task<ResponseDto?> GetBasketAsync(string subjectId)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            Sd = SD.ApiType.GET,
            Url = SD.BasketAPIBase + $"/{subjectId}"
        });
    }

    public async Task<ResponseDto?> AddProductIntoBasketAsync(string subjectId, int productId, int quantity)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            Sd = SD.ApiType.POST,
            Url = SD.BasketAPIBase + $"/api/basket?SubId={subjectId}&ProductId={productId}&Quantity={quantity}"
        });
    }
    public async Task<ResponseDto?> DeleteProductFromBasketAsync(string subjectId, int productId, int quantity)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            Sd = SD.ApiType.DELETE,
            Url = SD.BasketAPIBase + $"/api/basket?{subjectId}&{productId}&{quantity}"
        });
    }
    public async Task<ResponseDto?> CreateBasketAsync(string subjectId)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            Sd = SD.ApiType.POST,
            Url = SD.BasketAPIBase + $"/{subjectId}"
        });
    }
    public async Task<ResponseDto?> DeleteBasketAsync(string subjectId)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            Sd = SD.ApiType.POST,
            Url = SD.BasketAPIBase + $"/{subjectId}"
        });
    }
}
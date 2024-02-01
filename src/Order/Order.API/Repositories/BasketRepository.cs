using Newtonsoft.Json;
using Order.API.Models.Dto;
using Order.API.Repositories.Interfaces;
using Order.API.Utilities;

namespace Order.API.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IHttpClientFactory _httpClientFactory;

    public BasketRepository(IHttpClientFactory clientFactory)
    {
        _httpClientFactory = clientFactory;
    }
    public async Task<IEnumerable<ShoppingCartDto>> GetShoppingCarts(string login)
    {
        var client = _httpClientFactory.CreateClient("ShoppingCart");
        var response = await client.GetAsync($"{SD.BasketApiBase}/{login}");
        var apiContet = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContet);
        if (resp.IsSuccess)
        {
            return JsonConvert.DeserializeObject<IEnumerable<ShoppingCartDto>>(Convert.ToString(resp.Result));
        }
        return new List<ShoppingCartDto>();
    }
}
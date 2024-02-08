using IdentityModel.Client;
using Microsoft.EntityFrameworkCore;
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
    public async Task<ICollection<ShoppingCartDto>> GetShoppingCarts(string login)
    {
        var client = _httpClientFactory.CreateClient();
        var discoveryDocument = await client.GetDiscoveryDocumentAsync(SD.AuthApiBase);

        var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
        {
            Address = discoveryDocument.TokenEndpoint,

            ClientId = "order",
            ClientSecret = "order",
            Scope = "basket"
        });
        if (tokenResponse.IsError)
        {
            throw new Exception("CC ex");
        }
        client.SetBearerToken(tokenResponse.AccessToken);
        
        var response = await client.GetAsync($"{SD.BasketApiBase}/{login}");
        var apiContet = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContet);
        if (resp.IsSuccess)
        {
            return JsonConvert.DeserializeObject<ICollection<ShoppingCartDto>>(Convert.ToString(resp.Result)).AsQueryable().AsNoTracking().ToList();
        }
        return new List<ShoppingCartDto>();
    }
}
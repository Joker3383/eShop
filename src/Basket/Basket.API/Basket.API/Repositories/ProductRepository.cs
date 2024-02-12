using Basket.API.Models.Dto;
using Basket.API.Repositories.Interfaces;
using Basket.API.Utilities;
using IdentityModel.Client;
using Newtonsoft.Json;

namespace Basket.API.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductRepository(IHttpClientFactory clientFactory)
    {
        _httpClientFactory = clientFactory;
    }
    public async Task<ProductDto?> GetProductById(int productId)
    {
        var client = _httpClientFactory.CreateClient();
        var discoveryDocument = await client.GetDiscoveryDocumentAsync(SD.AuthApiBase);

        var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
        {
            Address = discoveryDocument.TokenEndpoint,

            ClientId = "basket",
            ClientSecret = "basketAPI",
            Scope = "product"
        });
        if (tokenResponse.IsError)
        {
            throw new Exception("CC ex");
        }
        
        client.SetBearerToken(tokenResponse.AccessToken);
        var response = await client.GetAsync($"{SD.CatalogApiBase}/api/product/{productId}");
        var apiContet = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContet);
        if (resp.IsSuccess)
        {
            return JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(resp.Result));
        }
        return null!;
    }
}
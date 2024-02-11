using AutoMapper;
using Basket.API.Models.Dto;
using Basket.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

[Authorize(Policy = "AuthenteficatedUser")]
[ApiController]
[Route("/api/basket")]
public class BasketController : ControllerBase
{
    private ResponseDto _response;
    private readonly IBasketService _basketService;

    public BasketController(IMapper mapper, IBasketService basketService)
    {
        _basketService = basketService;
        _response = new ResponseDto();
    }

    [HttpGet("/{login}")]
    [AllowAnonymous]
    public async Task<ResponseDto> GetByLogin(string login)
    {
        try
        {
            var shoppingCartsByLogin = await _basketService.GetShoppingCartsIntoBasket(login);
            if (shoppingCartsByLogin == null)
                throw new NullReferenceException($"By Login:{login} there aren`t products into basket");
            _response.Result = shoppingCartsByLogin;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }

        return _response;
    }
    
    [HttpPost("/{login}/{productId}")]
    [AllowAnonymous]
    public async Task<ResponseDto> AddProductIntoShoppingCartByLogin(string login, int productId)
    {
        try
        {
            var addedProduct = await _basketService.CreateShoppingCart(login, productId);
            if (addedProduct == null)
                throw new NullReferenceException("Product not added into basket");
            _response.Result = addedProduct;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }

        return _response;
    }
}
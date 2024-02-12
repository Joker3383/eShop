using AutoMapper;
using Basket.API.Models.Dto;
using Basket.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;


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

    [HttpGet("/{subId}")]
    public async Task<ResponseDto> GetByLogin(int subId)
    {
        try
        {
            var shoppingCartsByLogin = await _basketService.GetBasket(subId);
            if (shoppingCartsByLogin == null)
                throw new NullReferenceException($"By Login:{subId} there aren`t products into basket");
            _response.Result = shoppingCartsByLogin;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }

        return _response;
    }
    
    [HttpPost("/{subId}/{productId}/{quantity}")]
    public async Task<ResponseDto> AddProductIntoShoppingCartByLogin(int subId, int productId, int quantity)
    {
        try
        {
            var addedProduct = await _basketService.AddItemIntoBasketAsync(subId, productId, quantity);
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
    
    [HttpDelete("/{subId}/{productId}/{quantity}")]
    public async Task<ResponseDto> RemoveProductFromBasketAsync(int subId, int productId, int quantity)
    {
        try
        {
            var addedProduct = await _basketService.RemoveItemFromBasketAsync(subId, productId, quantity);
            if (addedProduct == null)
                throw new NullReferenceException("Product not deleted into basket");
            _response.Result = addedProduct;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }

        return _response;
    }
    [HttpPost("/{subId}")]
    public async Task<ResponseDto> CreateBasket(int subId)
    {
        try
        {
            var addedProduct = await _basketService.CreateBasket(subId);
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
    
    [HttpDelete("/{subId}")]
    public async Task<ResponseDto> DeleteBasket(int subId)
    {
        try
        {
            var addedProduct = await _basketService.DeleteBasket(subId);
            if (addedProduct == null)
                throw new NullReferenceException("Product not deleted into basket");
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
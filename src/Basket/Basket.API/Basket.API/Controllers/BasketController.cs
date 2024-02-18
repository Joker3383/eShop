using AutoMapper;
using Basket.API.Models.Dto;
using Basket.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

    [HttpGet("/{subId}")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
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
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    public async Task<ResponseDto> AddProductIntoShoppingCartByLogin([FromQuery]RequestDto requestDto)
    {
        try
        {
            if (!ModelState.IsValid)
                throw new ArgumentException("Model is not valid");
             
            var addedProduct = await _basketService.AddItemIntoBasketAsync(requestDto.SubId, requestDto.ProductId, requestDto.Quantity);
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
    
    [HttpDelete]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    public async Task<ResponseDto> RemoveProductFromBasketAsync([FromQuery]RequestDto requestDto)
    {
        try
        {
            var addedProduct = await _basketService.RemoveItemFromBasketAsync(requestDto.SubId, requestDto.ProductId, requestDto.Quantity);
            if (addedProduct == null)
                throw new NullReferenceException("Product not deleted from basket");
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
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
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
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
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
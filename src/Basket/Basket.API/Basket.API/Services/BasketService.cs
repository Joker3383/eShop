using AutoMapper;
using Basket.API.Data;
using Basket.API.Models;
using Basket.API.Models.Dto;
using Basket.API.Repositories.Interfaces;
using Basket.API.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Shared.CrudOperations;

namespace Basket.API.Services;

public class BasketService : IBasketService
{
    private readonly IProductRepository _productRepository;
    private readonly IBasketRepository _basketRepository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public BasketService(IProductRepository productRepository, IBasketRepository basketRepository, IMapper mapper, IMediator mediator)
    {
        _productRepository = productRepository;
        _basketRepository = basketRepository;
        _mapper = mapper;
        _mediator = mediator;
    }


    public async Task<Models.Basket> CreateBasket(int subId)
    {

        var basket = await _basketRepository.GetBasket(subId);
        if (basket == null)
        {
            await _mediator.Send(new CreateEntityCommand<Models.Basket, AppDbContext>(new Models.Basket
            {
                SubId = subId

            }));
        }

        return basket!;
    }

    public async Task<int> DeleteBasket(int subId)
    {
        var basket = await _basketRepository.GetBasket(subId);
        if (basket != null)
        {
            await _mediator.Send(new DeleteEntityCommand<Models.Basket, AppDbContext>(basket));
            return subId;
        }
        else
        {
            return subId;
        }
    }

    public async Task<Models.Basket> RemoveItemFromBasketAsync(int subId, int productId, int quantity)
    {
        var product = await _productRepository.GetProductById(productId);
        
        var basket = await _basketRepository.GetBasket(subId);
        if (basket != null)
        {
            basket.TotalCount -= product.Price * quantity;
            if (basket.Products != null && basket.Products.ContainsKey(productId))
            {
                basket.Products[productId] -= quantity;
                if (basket.Products[productId] <= 0)
                    basket.Products.Remove(productId);
            
                await _mediator.Send(new UpdateEntityCommand<Models.Basket,AppDbContext>(basket));
            }
        }
        return basket;
    }

    public async Task<Models.Basket> AddItemIntoBasketAsync(int subId, int productId, int quantity)
    {
        var product = await _productRepository.GetProductById(productId);

        if (product == null)
        {
            throw new NullReferenceException("This product doesn`t exist");
        }
        
        var basket = await _basketRepository.GetBasket(subId);
        if (basket != null)
        {
            basket.TotalCount += product.Price * quantity;
            
            if (basket.Products == null)
                basket.Products = new Dictionary<int, int>();

            if (basket.Products.ContainsKey(productId))
                basket.Products[productId] += quantity;
            else
                basket.Products[productId] = quantity;

            await _mediator.Send(new UpdateEntityCommand<Models.Basket,AppDbContext>(basket));
        }
        return basket;
    }

    public async Task<Models.Basket?> GetBasket(int subId)
    {
        return await _basketRepository.GetBasket(subId);
    }
}
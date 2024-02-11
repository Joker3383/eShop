using AutoMapper;
using Basket.API.Data;
using Basket.API.Models;
using Basket.API.Models.Dto;
using Basket.API.Repositories.Interfaces;
using Basket.API.Services.Interfaces;
using MediatR;
using Shared.CrudOperations;

namespace Basket.API.Services;

public class BasketService : IBasketService
{
    private readonly IProductRepository _productRepository;
    private readonly IBasketRepository _basketRepository;
    private readonly IMapper _mapper;
    private readonly ISender _sender;

    public BasketService(IProductRepository productRepository, IBasketRepository basketRepository, IMapper mapper, ISender sender)
    {
        _productRepository = productRepository;
        _basketRepository = basketRepository;
        _mapper = mapper;
        _sender = sender;
    }
    
    public async  Task<ShoppingCartDto> CreateShoppingCart(string login, int productId)
    {
        var allProducts = await _productRepository.GetProducts();
        if (allProducts == null)
        {
            throw new NullReferenceException("No avaible products to add");
        }

        var product = await _sender.Send(new GetEntityByIdQuery<Product, AppDbContext>(productId)); // usage

        if (product == null)
        {
            throw new NullReferenceException($"Don`t find product by id: {productId}");
        }
        
        var addedCart = await _basketRepository.Create(new ShoppingCart()
        {
            Login = login,
            Product = productById,
            ProductId = productId

        });
        
        
        return _mapper.Map<ShoppingCartDto>(addedCart);
    }

    public async Task<IEnumerable<ShoppingCartDto>> GetShoppingCartsIntoBasket(string login)
    {
        var allShoppingCarts =  _basketRepository.FindAll();

        if (allShoppingCarts == null)
        {
            throw new NullReferenceException("Basket is empty");
        }
        
        var selectedProducts = allShoppingCarts.Where(p => p.Login == login);
        if (selectedProducts == null)
        {
            throw new NullReferenceException($"There are no entries in your cart by login:{login}");
        }
        
        return _mapper.Map<IEnumerable<ShoppingCartDto>>(selectedProducts.ToList());
    }
    
}
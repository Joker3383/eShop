using AutoMapper;
using Basket.API.Models;
using Basket.API.Models.Dto;
using Basket.API.Repositories.Interfaces;
using Basket.API.Services.Interfaces;

namespace Basket.API.Services;

public class BasketService : IBasketService
{
    private readonly IProductRepository _productRepository;
    private readonly IBasketRepository _basketRepository;
    private IMapper _mapper;

    public BasketService(IProductRepository productRepository, IBasketRepository basketRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _basketRepository = basketRepository;
        _mapper = mapper;
    }
    
    public async  Task<ShoppingCartDto> CreateShoppingCart(string login, int productId)
    {
        var products = await _productRepository.GetProducts();

        var productById = products.FirstOrDefault(p => p.ProductId == productId);
        
        
        var cart = new ShoppingCart()
        {
            Login = login,
            Product = productById!,
            ProductId = productId

        };
        
        var addedCart = await _basketRepository.Create(cart);
        
        return _mapper.Map<ShoppingCartDto>(addedCart);
    }

    public async Task<IEnumerable<ShoppingCartDto>> GetProductsIntoBasket(string login)
    {
        var allProducts =  _basketRepository.FindAll();
        
        var selectedProducts = allProducts.Where(p => p.Login == login);
        
        return _mapper.Map<IEnumerable<ShoppingCartDto>>(selectedProducts.ToList());
    }
    
}
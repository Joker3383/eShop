using AutoMapper;
using Basket.API.Models.Dto;
using Basket.API.Repositories.Interfaces;
using Basket.API.Services;
using Catalog.API.Data;
using MediatR;
using Moq;
using Shared.CrudOperations;
using AppDbContext = Basket.API.Data.AppDbContext;

namespace Basket.UnitTests;

public class BasketServiceUnitTests
{
    private readonly Mock<IProductRepository> _basketRepositoryMock;
    private readonly Mock<IMediator> _mediatorMock;
    public BasketServiceUnitTests()
    {

        _mediatorMock = new Mock<IMediator>();
        _basketRepositoryMock = new Mock<IProductRepository>();
    }
    [Fact]
    public async Task CreateBasket_ValidSubId_ReturnsBasket_Success()
    {
        int subId = 123;
        var excectedBasket = new API.Models.Basket { Id = 0, SubId = 123, TotalCount = 0, Products = null };
        var nullProduct = new API.Models.Basket();
        
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetEntityByIdQuery<API.Models.Basket, AppDbContext>>(), default))
            .ReturnsAsync(excectedBasket);
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateEntityCommand<API.Models.Basket, AppDbContext>>(), default))
            .Returns(Task.CompletedTask);

        var basketService = new BasketService( _basketRepositoryMock.Object, _mediatorMock.Object);

        var result = await basketService.CreateBasket(subId);
        
        Assert.Equivalent(excectedBasket, result);
        
    }
    [Fact]
    public async Task CreateBasket_ValidSubId_ThrowExcept_Failure()
    {
        int subId = 123;
        var nullProduct = new API.Models.Basket();
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetEntityByIdQuery<API.Models.Basket, AppDbContext>>(), default))
            .ReturnsAsync(nullProduct);
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateEntityCommand<API.Models.Basket, AppDbContext>>(), default))
            .ThrowsAsync(new  MediatorException("Failed"));
        var basketService = new BasketService( _basketRepositoryMock.Object, _mediatorMock.Object);
        
        
        await Assert.ThrowsAsync<MediatorException>(async () => await basketService.CreateBasket(subId));
    }

    [Fact]
    public async Task DeleteBasket_FindBasket_ReturnDeletedBasket_Success()
    {
        int subId = 123;
        var excectedBasket = new API.Models.Basket { Id = 0, SubId = 123, TotalCount = 0, Products = null };
        var nullProduct = new API.Models.Basket();
        
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetEntityByIdQuery<API.Models.Basket, AppDbContext>>(), default))
            .ReturnsAsync(excectedBasket);
        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteEntityCommand<API.Models.Basket, AppDbContext>>(), default))
            .Returns(Task.CompletedTask);

        var basketService = new BasketService( _basketRepositoryMock.Object, _mediatorMock.Object);

        var result = await basketService.DeleteBasket(subId);
        
        Assert.Equivalent(subId, result);
    }
    
    [Fact]
    public async Task DeleteBasket_FindBasket_ThrowExcept_Failure()
    {
        int subId = 123;
        var nullBasket = new API.Models.Basket();
        
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetEntityByIdQuery<API.Models.Basket, AppDbContext>>(), default))
            .ReturnsAsync(nullBasket);
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateEntityCommand<API.Models.Basket, AppDbContext>>(), default))
            .ThrowsAsync(new  MediatorException("Failed"));

        var basketService = new BasketService( _basketRepositoryMock.Object, _mediatorMock.Object);
        
        await Assert.ThrowsAsync<MediatorException>(async () => await basketService.CreateBasket(subId));
    }
    
    [Fact]
    public async Task GetBasketById_FindById_Success()
    {
        var subId = 123;
        var product = new API.Models.Basket();
        _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<GetEntityBySubIdQuery<API.Models.Basket, AppDbContext>>(), default))
            .ReturnsAsync(product);
        
        var service = new BasketService( _basketRepositoryMock.Object, _mediatorMock.Object);
        var result = await service.GetBasket(subId);
        
        Assert.NotNull(result);
    }
    [Fact]
    public async Task GetBasketById_FindById_Failure()
    {
        var subId = 123;
        var product = new API.Models.Basket();
        _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<GetEntityBySubIdQuery<API.Models.Basket, AppDbContext>>(), default))
            .ReturnsAsync((API.Models.Basket)null);
        
        var service = new BasketService( _basketRepositoryMock.Object, _mediatorMock.Object);
        var result = await service.GetBasket(subId);
        
        Assert.Null(result);

    }

    [Fact]
    public async Task AddItemIntoBasketAsync_Success()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockMediator = new Mock<IMediator>();
        var basketService = new BasketService(mockProductRepository.Object, mockMediator.Object);

        int subId = 123;
        int productId = 456;
        int quantity = 2;
        var productDto = new ProductDto { ProductId = productId, Price = 10 }; 

        var basket = new API.Models.Basket { SubId = subId, TotalCount = 0, Products = new Dictionary<int, int>() };

        mockProductRepository.Setup(repo => repo.GetProductById(productId))
            .ReturnsAsync(productDto);

        mockMediator.Setup(m => m.Send(It.IsAny<GetEntityBySubIdQuery<API.Models.Basket, AppDbContext>>(), default))
            .ReturnsAsync(basket);

        // Act
        var result = await basketService.AddItemIntoBasketAsync(subId, productId, quantity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(basket, result);
        Assert.Equal(quantity * productDto.Price, result.TotalCount);
        Assert.True(result.Products.ContainsKey(productId));
        Assert.Equal(quantity, result.Products[productId]);
    }

    [Fact]
    public async Task AddItemIntoBasketAsync_ProductNotFound()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockMediator = new Mock<IMediator>();
        var basketService = new BasketService(mockProductRepository.Object, mockMediator.Object);

        int subId = 123;
        int productId = 456;
        int quantity = 2;

        mockProductRepository.Setup(repo => repo.GetProductById(productId))
            .ReturnsAsync((ProductDto)null);

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => basketService.AddItemIntoBasketAsync(subId, productId, quantity));
    }
    
   [Fact]
    public async Task RemoveItemFromBasketAsync_Success()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockMediator = new Mock<IMediator>();
        var basketService = new BasketService(mockProductRepository.Object, mockMediator.Object);

        int subId = 123;
        int productId = 456;
        int quantity = 2;
        var product = new ProductDto { ProductId = productId, Price = 10 }; // Sample product

        var basket = new API.Models.Basket { SubId = subId, TotalCount = 20, Products = new Dictionary<int, int> { { productId, quantity } } };

        mockProductRepository.Setup(repo => repo.GetProductById(productId))
                             .ReturnsAsync(product);

        mockMediator.Setup(m => m.Send(It.IsAny<GetEntityBySubIdQuery<API.Models.Basket, AppDbContext>>(), default))
                    .ReturnsAsync(basket);

        // Act
        var result = await basketService.RemoveItemFromBasketAsync(subId, productId, quantity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(basket, result);
        Assert.Equal(0, result.TotalCount);
        Assert.False(result.Products.ContainsKey(productId));
    }
    
    [Fact]
    public async Task RemoveItemFromBasketAsync_ProductNotInBasket()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockMediator = new Mock<IMediator>();
        var basketService = new BasketService(mockProductRepository.Object, mockMediator.Object);

        int subId = 123;
        int productId = 456;
        int quantity = 2;

        var basket = new API.Models.Basket { SubId = subId, TotalCount = 20, Products = new Dictionary<int, int>() };

        mockProductRepository.Setup(repo => repo.GetProductById(productId))
                             .ReturnsAsync(new ProductDto()); 

        mockMediator.Setup(m => m.Send(It.IsAny<GetEntityBySubIdQuery<API.Models.Basket, AppDbContext>>(), default))
                    .ReturnsAsync(basket); 

        // Act
        var result = await basketService.RemoveItemFromBasketAsync(subId, productId, quantity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(basket, result);
        Assert.Equal(20, result.TotalCount); // TotalCount remains the same
        Assert.False(result.Products.ContainsKey(productId)); // Product is not added to basket
    }
}
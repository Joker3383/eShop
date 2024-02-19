using AutoMapper;
using Catalog.API.Data;
using Catalog.API.Models.Dto;
using Catalog.API.Services;
using Catalog.API.Services.Interfaces;
using MediatR;
using Moq;
using Shared.CrudOperations;

namespace Catalog.UnitTests;

public class ProductServiceUnitTests
{
    

    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IMapper> _mapperMock;
    

    public ProductServiceUnitTests()
    {

        _mediatorMock = new Mock<IMediator>();
        _mapperMock = new Mock<IMapper>();
    }
    [Fact]
    public async Task GetProducts_FetchProducts_Success()
    {
        int subId = 123;
        var products = new List<Product>();

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetEntitiesQuery<Product, AppDbContext>>(), default)).ReturnsAsync(products.AsQueryable);
        
        var service = new ProductService( _mapperMock.Object, _mediatorMock.Object);
        var result = service.GetProducts();
        
        Assert.NotNull(result.Result);
    }
    
    [Fact]
    public async Task GetProducts_FetchProducts_Failure()
    {
        // Arrange
        var mockMediator = new Mock<IMediator>();
        
        IQueryable<Product> nullProducts = null;

        mockMediator.Setup(m => m.Send(It.IsAny<GetEntitiesQuery<Product, AppDbContext>>(), default))
            .ReturnsAsync(nullProducts);
        var service = new ProductService( _mapperMock.Object, _mediatorMock.Object);

        var result = await service.GetProducts();


        Assert.Equal(0,result.Count());
    }

    [Fact]
    public async Task GetProductById_FindByIdProduct_Success()
    {
        
        
        var subId = 123;
        var product = new Product();
        _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<GetEntityByIdQuery<Product, AppDbContext>>(), default)).ReturnsAsync(product);
        
        var service = new ProductService( _mapperMock.Object, _mediatorMock.Object);
        var result = service.GetProductById(subId);
        
        Assert.NotNull(result);
    }
    [Fact]
    public async Task GetProductById_FindByIdProduct_Failure()
    {
        var subId = 123;
        _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<GetEntityByIdQuery<Product, AppDbContext>>(), default)).ReturnsAsync((Product)null);
        
        var service = new ProductService( _mapperMock.Object, _mediatorMock.Object);
        var result = service.GetProductById(subId);
        
        Assert.Null(result.Result);

    }
    
    [Fact]
    public async Task CreateProduct_MapProduct_Success()
    {
        var productDto = new ProductDto
            { CategoryName = "", Name = "", Id = 123, Description = "", Price = 0, ImageUrl = "" };
        var expectedProduct = new Product
            { CategoryName = "", Name = "", Id = 123, Description = "", Price = 0, ImageUrl = "" };
        _mapperMock.Setup(m => m.Map<ProductDto, Product>(productDto))
            .Returns((expectedProduct));
        
        
        var productService = new ProductService(_mapperMock.Object, _mediatorMock.Object);
        var result = await productService.CreateProduct(productDto);
        
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task CreateProduct_MapProduct_Failure()
    {
        var productDto = new ProductDto
            { CategoryName = "", Name = "", Id = 123, Description = "", Price = 0, ImageUrl = "" };
        _mapperMock.Setup(m => m.Map<ProductDto, Product>(productDto))
            .Returns((Product)null);
        
        
        var productService = new ProductService(_mapperMock.Object, _mediatorMock.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(() => productService.CreateProduct(productDto));

    }
    
    [Fact]
    public async Task UpdateProduct_SendCommand_Success()
    {
        var productDto = new ProductDto
            { CategoryName = "", Name = "", Id = 123, Description = "", Price = 0, ImageUrl = "" };
        var expectedProduct = new Product
            { CategoryName = "", Name = "", Id = 123, Description = "", Price = 0, ImageUrl = "" };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetEntityByIdQuery<Product, AppDbContext>>(), default))
            .ReturnsAsync(expectedProduct);
        _mapperMock.Setup(m => m.Map<ProductDto, Product>(productDto))
            .Returns((expectedProduct));
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateEntityCommand<Product, AppDbContext>>(), default))
            .Returns(Task.CompletedTask);
        var productService = new ProductService(_mapperMock.Object, _mediatorMock.Object);

        var result = await productService.UpdateProduct(productDto);
        
        Assert.Equal(expectedProduct, result);

    }
    
    [Fact]
    public async Task UpdateProduct_SendCommand_Failure()
    {
        var productDto = new ProductDto
            { CategoryName = "", Name = "", Id = 123, Description = "", Price = 0, ImageUrl = "" };
        var expectedProduct = new Product
            { CategoryName = "", Name = "", Id = 123, Description = "", Price = 0, ImageUrl = "" };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetEntityByIdQuery<Product, AppDbContext>>(), default))
            .ReturnsAsync(expectedProduct);
        _mapperMock.Setup(m => m.Map<ProductDto, Product>(productDto))
            .Returns((expectedProduct));
        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateEntityCommand<Product, AppDbContext>>(), default))
            .ThrowsAsync(new MediatorException("Failed to send command"));
        var productService = new ProductService(_mapperMock.Object, _mediatorMock.Object);



        await Assert.ThrowsAsync<MediatorException>(() => productService.UpdateProduct(productDto));
    }
    
    [Fact]
    public async Task DeleteProduct_SendCommand_Success()
    {
        int id = 123;
        var expectedProduct = new Product
            { CategoryName = "", Name = "", Id = 123, Description = "", Price = 0, ImageUrl = "" };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetEntityByIdQuery<Product, AppDbContext>>(), default))
            .ReturnsAsync(expectedProduct);
        
        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteEntityCommand<Product, AppDbContext>>(), default))
            .Returns(Task.CompletedTask);
        
        var productService = new ProductService(_mapperMock.Object, _mediatorMock.Object);

        var result = await productService.DaleteProduct(id);
        
        Assert.NotNull(expectedProduct);

    }
    
    [Fact]
    public async Task DeleteProduct_SendCommand_Failure()
    {
        int id = 123;
        var expectedProduct = new Product
            { CategoryName = "", Name = "", Id = 123, Description = "", Price = 0, ImageUrl = "" };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetEntityByIdQuery<Product, AppDbContext>>(), default))
            .ReturnsAsync(expectedProduct);
        
        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteEntityCommand<Product, AppDbContext>>(), default))
            .ThrowsAsync(new MediatorException("Failed to send command"));
        
        var productService = new ProductService(_mapperMock.Object, _mediatorMock.Object);

        
        await Assert.ThrowsAsync<MediatorException>(() => productService.DaleteProduct(id));

    }

}
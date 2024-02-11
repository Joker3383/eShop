using AutoMapper;
using Catalog.API.Data;
using Catalog.API.Models.Dto;
using Catalog.API.Repositories.Interfaces;
using Catalog.API.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.CrudOperations;

namespace Catalog.API.Services;

public class ProductService : IProductService
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    
    public ProductService(IMapper mapper, IMediator mediator) 
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<Product> CreateProduct(ProductDto carDto)
    {
        if (carDto == null)
        {
            throw new ArgumentNullException(nameof(carDto), "CarDto cannot be null");
        }
        
        var car = _mapper.Map<ProductDto, Product>(carDto);
        
        if (car == null)
        {
            throw new InvalidOperationException("Mapping from CarDto to Car failed");
        }
        try
        {
            await _mediator.Send(new CreateEntityCommand<Product,AppDbContext>(car));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to create car", ex);
        }

        return car;
    }

    public async Task<Product> UpdateProduct(ProductDto productDto)
    {
        if (productDto == null)
        {
            throw new ArgumentNullException(nameof(productDto), "ProductDto cannot be null");
        }

        var existingProduct = await _mediator.Send(new GetEntityByIdQuery<Product,AppDbContext>(productDto.Id));
        if (existingProduct == null)
        {
            throw new InvalidOperationException($"Product with ID {productDto.Id} not found");
        }
        
        _mapper.Map(productDto, existingProduct);
        
        try
        {
            await _mediator.Send(new UpdateEntityCommand<Product, AppDbContext>(existingProduct));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to update product", ex);
        }
        
        return existingProduct;
    }

    public async Task<Product> DaleteProduct(int id)
    {
        var product = await GetProductById(id);

        if (product == null)
        {
            throw new InvalidOperationException($"Product with ID {id} not found");
        }

        try
        {
            await _mediator.Send(new DeleteEntityCommand<Product,AppDbContext>(product));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to delete Product", ex);
        }

        return product;
    }

    public async Task<IQueryable<Product>> GetProducts()
    {
        var products = await _mediator.Send(new GetEntitiesQuery<Product,AppDbContext>());
        return products.AsQueryable();
    }

    public async Task<Product?> GetProductById(int id)
    {
        return await _mediator.Send(new GetEntityByIdQuery<Product,AppDbContext>(id));
    }
}
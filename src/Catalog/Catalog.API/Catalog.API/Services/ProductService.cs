using AutoMapper;
using Catalog.API.Data;
using Catalog.API.Models.Dto;
using Catalog.API.Repositories.Interfaces;
using Catalog.API.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared;
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

    public async Task<Product> CreateProduct(ProductDto productDto)
    {
        if (productDto == null)
        {
            throw new ArgumentNullException(nameof(productDto), "CarDto cannot be null");
        }
        
        var product = _mapper.Map<ProductDto, Product>(productDto);
        
        if (product == null)
        {
            throw new InvalidOperationException("Mapping from CarDto to Car failed");
        }
        try
        {
            await _mediator.Send(new CreateEntityCommand<Product,AppDbContext>(product));
        }
        catch (MediatorException)
        {
            throw new MediatorException("Failed to create car");
        }

        return product;
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
            throw new MediatorException($"Product with ID {productDto.Id} not found");
        }
        
        _mapper.Map(productDto, existingProduct);
        
        try
        {
            await _mediator.Send(new UpdateEntityCommand<Product, AppDbContext>(existingProduct));
        }
        catch (MediatorException)
        {
            throw new MediatorException("Failed to update product");
        }
        
        return existingProduct;
    }

    public async Task<Product> DaleteProduct(int id)
    {
        var product = await GetProductById(id);

        if (product == null)
        {
            throw new MediatorException($"Product with ID {id} not found");
        }

        try
        {
            await _mediator.Send(new DeleteEntityCommand<Product,AppDbContext>(product));
        }
        catch (MediatorException ex)
        {
            throw new MediatorException("Failed to delete Product");
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
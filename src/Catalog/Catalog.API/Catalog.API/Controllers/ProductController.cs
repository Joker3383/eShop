using AutoMapper;
using Catalog.API.Models.Dto;
using Catalog.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Controllers;


[ApiController]
[Authorize(Policy = "AuthenteficatedUser")]
[Route("/api/product")]
public class ProductController : ControllerBase
{
    private  readonly IProductService _productService;
    private ResponseDto _response;
    private IMapper _mapper;
    
    public ProductController(IProductService carService, IMapper mapper)
    {
        _productService = carService;
        _response = new ResponseDto();
        _mapper = mapper;
    }

    [HttpGet]
    
    public async Task<ResponseDto> GetProducts()
    {
        try
        {
            var products = await _productService.GetProducts();
            if (products == null)
                throw new NullReferenceException("No products available");
            _response.Result = _mapper.Map<IEnumerable<ProductDto>>(products.ToList());
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }

        return _response;
    }
    
    [HttpGet]
    
    [Route("{productId:int}")]
    public async Task<ResponseDto> GetProductById(int productId)
    {
        try
        {
            var productById = await _productService.GetProductById(productId);
            if (productById == null)
                throw new NullReferenceException($"Product by id: {productId} not available");
            _response.Result = _mapper.Map<ProductDto>(productById);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }

        return _response;
    }
    
    [HttpPost]
    public async Task<ResponseDto> PostProduct(ProductDto productDto)
    {
        try
        {
            var product = await _productService.CreateProduct(productDto);
            if (product == null)
                throw new NullReferenceException($"The product was not added");
            _response.Result = _mapper.Map<ProductDto>(product);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }

        return _response;
    }
    
    [HttpPut]
    public async Task<ResponseDto> UpdateProduct(ProductDto productDto)
    {
        try
        {
            var updatedProduct = await _productService.UpdateProduct(productDto);
            if (updatedProduct == null)
                throw new NullReferenceException($"Product by id: {productDto.Id} not updated");
            _response.Result = _mapper.Map<ProductDto>(updatedProduct);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }

        return _response;
    }
    
    [HttpDelete]
    [Route("{productId:int}")]
    public async Task<ResponseDto> DeleteProduct(int productId)
    {
        try
        {
            var productById = await _productService.DaleteProduct(productId);
            if (productById == null)
                throw new NullReferenceException($"Product by id: {productId} not deleted");
            _response.Result = _mapper.Map<ProductDto>(productById);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }

        return _response;
    }
}
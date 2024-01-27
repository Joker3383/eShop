using AutoMapper;
using Catalog.API.Models.Dto;
using Catalog.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;


[ApiController]
[Route("/api/bff")]
public class BffController : ControllerBase
{
    private  readonly IProductService _productService;
    private ResponseDto _response;
    private IMapper _mapper;
    
    public BffController(IProductService carService, IMapper mapper)
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
}
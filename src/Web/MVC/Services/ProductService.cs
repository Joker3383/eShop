namespace MVC.Services;

public class ProductService : IProductService
{
    private readonly IBaseService _baseService;
    public ProductService(IBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task<ResponseDto?> CreateProductsAsync(ProductDto productDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            Sd = SD.ApiType.POST,
            Data=productDto,
            Url = SD.ProductAPIBase + "/api/product" ,

        });
    }

    public async Task<ResponseDto?> DeleteProductsAsync(int id)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            Sd = SD.ApiType.DELETE,
            Url = SD.ProductAPIBase + "/api/product/" + id
        }); 
    }

    public async Task<ResponseDto?> GetAllProductsAsync()
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            Sd = SD.ApiType.GET,
            Url = SD.ProductAPIBase + "/api/bff"
        });
    }
    
    public async Task<ResponseDto?> GetProductByIdAsync(int id)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            Sd = SD.ApiType.GET,
            Url = SD.ProductAPIBase + "/api/bff/" + id
        });
    }

    public async Task<ResponseDto?> UpdateProductsAsync(ProductDto productDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            Sd = SD.ApiType.PUT,
            Data = productDto,
            Url = SD.ProductAPIBase + "/api/product",
        });
    }
}
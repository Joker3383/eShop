using MVC.Models;
using MVC.Services.Interfaces;

namespace MVC.Services;

public class OrderService : IOrderService
{
    private IBaseService _baseService;
    public OrderService(IBaseService baseService)
    {
        _baseService = baseService;
    }
    public async Task<ResponseDto?> GetOrdersAsync(string subjectId)
    {
        return await _baseService.SendAsync(new RequestDto
        {
            Sd = SD.ApiType.GET,
            Url = SD.OrderAPIBase + $"/{subjectId}"
        });
    }

    public async Task<ResponseDto?> CreateOrderAsync(string subjectId)
    {
        return await _baseService.SendAsync(new RequestDto
        {
            Sd = SD.ApiType.POST,
            Url = SD.OrderAPIBase + $"/{subjectId}"
        });
    }
    
    public async Task<ResponseDto?> CreateOrderAsync(int Id)
    {
        return await _baseService.SendAsync(new RequestDto
        {
            Sd = SD.ApiType.DELETE,
            Url = SD.OrderAPIBase + $"/{Id}"
        });
    }
}
using MVC.Models;

namespace MVC.Services.Interfaces;

public interface IBasketService
{
    Task<ResponseDto?> GetShoppingCartsAsync(string subjectId);
    Task<ResponseDto?> AddShoppingCartAsync(string subjectId, int productId);
}
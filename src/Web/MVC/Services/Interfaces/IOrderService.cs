﻿using MVC.Models;

namespace MVC.Services.Interfaces;

public interface IOrderService
{
    Task<ResponseDto?> GetOrdersAsync(string subjectId);
    Task<ResponseDto?> CreateOrderAsync(string subjectId);
}
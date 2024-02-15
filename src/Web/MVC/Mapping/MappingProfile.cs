using AutoMapper;
using MVC.Controllers;
using MVC.Models;

namespace MVC.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ProductDto, ProductRequest>();
        CreateMap<ProductRequest, ProductDto>();
    }
}
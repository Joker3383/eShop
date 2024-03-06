using AutoMapper;
using MVC.Controllers;


namespace MVC.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ProductDto, ProductRequest>();
        CreateMap<ProductRequest, ProductDto>();
    }
}
using AutoMapper;


namespace Basket.API.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Models.Basket, BasketDto>();
        CreateMap<BasketDto, Models.Basket>();
    }
}
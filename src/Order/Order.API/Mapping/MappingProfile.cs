using AutoMapper;
using Order.API.Models.Dto;
using Order.API.Models;

namespace Order.API.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Models.Order, OrderDto>();
        CreateMap<OrderDto, Models.Order>();
    }
}
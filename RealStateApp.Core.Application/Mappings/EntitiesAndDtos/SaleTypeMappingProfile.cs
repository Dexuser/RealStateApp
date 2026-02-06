using AutoMapper;
using RealStateApp.Core.Application.Dtos.SaleType;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Mappings.EntitiesAndDtos;

public class SaleTypeMappingProfile : Profile
{
    public SaleTypeMappingProfile()
    {
        CreateMap<SaleType, SaleTypeDto>().ReverseMap();
        CreateMap<SaleType, SaleTypeApiDto>().ReverseMap();
    }
}
using AutoMapper;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Mappings.EntitiesAndDtos;

public class PropertyMappingProfile : Profile
{
    public PropertyMappingProfile()
    {
        CreateMap<Property, PropertyDto>().ReverseMap();
        CreateMap<Property, PropertyApiDto>()
            .ForMember(dest => dest.PropertyType, opt => opt.MapFrom(src => src.PropertyType!.Name))
            .ForMember(dest => dest.SaleType, opt => opt.MapFrom(src => src.SaleType!.Name))
            .ForMember(dest => dest.PropertyImprovements, opt => opt.MapFrom(src => src.PropertyImprovements.Select(pi => pi.Improvement.Name)));
        
    }
    
}
using AutoMapper;
using RealStateApp.Core.Application.Dtos.PropertyType;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Mappings.EntitiesAndDtos;

public class PropertyTypeMappingProfile : Profile
{
    public PropertyTypeMappingProfile()
    {
        CreateMap<PropertyType, PropertyTypeDto>().ReverseMap();
    }
    
}
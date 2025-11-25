using AutoMapper;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Mappings.EntitiesAndDtos;

public class PropertyMappingProfile : Profile
{
    public PropertyMappingProfile()
    {
        CreateMap<Property, PropertyDto>();
    }
    
}
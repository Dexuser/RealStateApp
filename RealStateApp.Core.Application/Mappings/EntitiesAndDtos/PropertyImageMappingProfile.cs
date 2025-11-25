using AutoMapper;
using RealStateApp.Core.Application.Dtos.PropertyImage;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Mappings.EntitiesAndDtos;

public class PropertyImageMappingProfile : Profile
{

    public PropertyImageMappingProfile()
    {
        CreateMap<PropertyImage, PropertyImageDto>();
    }
}
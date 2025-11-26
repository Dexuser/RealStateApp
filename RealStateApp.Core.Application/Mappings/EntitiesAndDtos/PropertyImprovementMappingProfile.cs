using AutoMapper;
using RealStateApp.Core.Application.Dtos.PropertyImprovement;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Mappings.EntitiesAndDtos;

public class PropertyImprovementMappingProfile : Profile
{
    public PropertyImprovementMappingProfile()
    {
        CreateMap<PropertyImprovement, PropertyImprovementDto>().ReverseMap();
    }
    
}
using AutoMapper;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.ViewModels.Property;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Mappings.DtosAndViewModels;

public class PropertyDtoMappingProfile : Profile
{
    public PropertyDtoMappingProfile()
    {
        CreateMap<PropertyDto, PropertyViewModel>().ReverseMap();
    }
    
}
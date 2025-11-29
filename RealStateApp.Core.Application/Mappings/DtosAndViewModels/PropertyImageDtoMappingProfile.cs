using AutoMapper;
using RealStateApp.Core.Application.Dtos.PropertyImage;
using RealStateApp.Core.Application.ViewModels.PropertyImage;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Mappings.DtosAndViewModels;

public class PropertyImageDtoMappingProfile : Profile
{

    public PropertyImageDtoMappingProfile()
    {
        CreateMap<PropertyImageDto, PropertyImageViewModel>().ReverseMap();
    }
}
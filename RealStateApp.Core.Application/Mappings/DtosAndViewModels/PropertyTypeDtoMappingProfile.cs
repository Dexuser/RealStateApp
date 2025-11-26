using AutoMapper;
using RealStateApp.Core.Application.Dtos.PropertyType;
using RealStateApp.Core.Application.ViewModels.PropertyType;

namespace RealStateApp.Core.Application.Mappings.DtosAndViewModels;

public class PropertyTypeDtoMappingProfile : Profile
{
    public PropertyTypeDtoMappingProfile()
    {
        CreateMap<PropertyTypeDto, PropertyTypeViewModel>().ReverseMap();
        CreateMap<PropertyTypeWithCountDto, PropertyTypeWithCountViewModel >().ReverseMap();
        CreateMap<CreatePropertyTypeViewModel, PropertyTypeDto>().ReverseMap();
        CreateMap<EditPropertyTypeViewModel, PropertyTypeDto>().ReverseMap();
    }
}
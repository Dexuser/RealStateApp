using AutoMapper;
using RealStateApp.Core.Application.Dtos.PropertyImprovement;
using RealStateApp.Core.Application.ViewModels.PropertyImprovement;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Mappings.DtosAndViewModels;

public class PropertyImprovementDtoMappingProfile : Profile
{
    public PropertyImprovementDtoMappingProfile()
    {
        CreateMap<PropertyImprovementDto, PropertyImprovementViewModel>().ReverseMap();
    }
    
}
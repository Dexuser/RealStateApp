using AutoMapper;
using RealStateApp.Core.Application.Dtos.Improvement;
using RealStateApp.Core.Application.Dtos.PropertyType;
using RealStateApp.Core.Application.ViewModels.Improvement;
using RealStateApp.Core.Application.ViewModels.PropertyType;

namespace RealStateApp.Core.Application.Mappings.DtosAndViewModels;

public class ImprovementDtoMappingProfile : Profile
{
    public ImprovementDtoMappingProfile()
    {
        CreateMap<ImprovementDto, ImprovementViewModel>().ReverseMap();
        CreateMap<CreateImprovementViewModel, ImprovementDto>().ReverseMap();
        CreateMap<EditImprovementViewModel, ImprovementDto>().ReverseMap();
    }
}
using AutoMapper;
using RealStateApp.Core.Application.Dtos.FavoriteProperty;
using RealStateApp.Core.Application.ViewModels.FavoriteProperty;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Mappings.DtosAndViewModels;

public class FavoritePropertyDtoMappingProfile : Profile
{
    public FavoritePropertyDtoMappingProfile()
    {
        CreateMap<FavoritePropertyDto, FavoritePropertyViewModel>().ReverseMap();
    }
}
using AutoMapper;
using RealStateApp.Core.Application.Dtos.FavoriteProperty;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Mappings.EntitiesAndDtos;

public class FavoritePropertyMappingProfile : Profile
{
    public FavoritePropertyMappingProfile()
    {
        CreateMap<FavoriteProperty, FavoritePropertyDto>().ReverseMap();
    }
}
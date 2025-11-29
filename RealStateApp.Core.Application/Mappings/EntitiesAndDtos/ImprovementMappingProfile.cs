using AutoMapper;
using RealStateApp.Core.Application.Dtos.Improvement;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Mappings.EntitiesAndDtos;

public class ImprovementMappingProfile : Profile
{
    public ImprovementMappingProfile()
    {
        CreateMap<Improvement, ImprovementDto>().ReverseMap();
    }
}
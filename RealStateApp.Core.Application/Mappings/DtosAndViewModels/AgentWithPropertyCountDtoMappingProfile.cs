using AutoMapper;
using RealStateApp.Core.Application.Dtos.Agent;
using RealStateApp.Core.Application.ViewModels.Agent;

namespace RealStateApp.Core.Application.Mappings.DtosAndViewModels;

public class AgentWithPropertyCountDtoMappingProfile : Profile
{
    public AgentWithPropertyCountDtoMappingProfile()
    {
        CreateMap<AgentWithPropertyCountDto, AgentWithPropertyCountViewModel>().ReverseMap();
    }
}
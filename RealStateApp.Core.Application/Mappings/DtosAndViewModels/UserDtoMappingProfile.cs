using AutoMapper;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.ViewModels.Login;

namespace RealStateApp.Core.Application.Mappings.DtosAndViewModels;

public class UserDtoMappingProfile : Profile
{

    public UserDtoMappingProfile()
    {
        CreateMap<UserSaveDto, UserDto>().ReverseMap();
        CreateMap<CreateClientOrAgentViewModel, UserSaveDto>().ReverseMap();
    }
}
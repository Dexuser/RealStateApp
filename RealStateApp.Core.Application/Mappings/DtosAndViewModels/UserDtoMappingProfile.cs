using AutoMapper;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.ViewModels.Login;
using RealStateApp.Core.Application.ViewModels.User;

namespace RealStateApp.Core.Application.Mappings.DtosAndViewModels;

public class UserDtoMappingProfile : Profile
{

    public UserDtoMappingProfile()
    {
        CreateMap<UserDto, UserViewModel>().ReverseMap();
        CreateMap<UserSaveDto, UserDto>().ReverseMap();
        CreateMap<CreateClientOrAgentViewModel, UserSaveDto>().ReverseMap();
    }
}

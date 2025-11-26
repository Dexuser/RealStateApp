using AutoMapper;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.ViewModels.Admin;
using RealStateApp.Core.Application.ViewModels.Agent;
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
        CreateMap<CreateAdminViewModel, UserSaveDto>().ReverseMap();
        CreateMap<EditAdminViewModel, UserSaveDto>().ReverseMap();
        CreateMap<UserDto, CreateAdminViewModel>().ReverseMap();
        CreateMap<UserDto, EditAdminViewModel>()
            .ReverseMap()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
    }
}

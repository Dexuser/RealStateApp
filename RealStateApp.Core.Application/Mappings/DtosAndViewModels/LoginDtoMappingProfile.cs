using ArtemisBanking.Core.Application.Dtos.Login;
using AutoMapper;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.ViewModels.Login;

namespace RealStateApp.Core.Application.Mappings.DtosAndViewModels;

public class LoginDtoMappingProfile :  Profile
{
    public LoginDtoMappingProfile()
    {
        CreateMap<LoginViewModel, LoginDto>().ReverseMap();
        CreateMap<EditUserViewModel, UserSaveDto>().ReverseMap();
    }
    
}
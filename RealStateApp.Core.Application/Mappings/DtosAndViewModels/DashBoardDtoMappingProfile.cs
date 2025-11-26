using AutoMapper;
using RealStateApp.Core.Application.Dtos.DashBoard;
using RealStateApp.Core.Application.ViewModels.DashBoard;

namespace RealStateApp.Core.Application.Mappings.DtosAndViewModels;

public class DashBoardDtoMappingProfile : Profile
{
    public DashBoardDtoMappingProfile()
    {
        CreateMap<AdminDashBoardDto, AdminDashBoardViewModel>().ReverseMap();
    }
    
}
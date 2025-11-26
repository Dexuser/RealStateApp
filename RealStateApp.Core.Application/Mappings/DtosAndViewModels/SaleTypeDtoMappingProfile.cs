using AutoMapper;
using RealStateApp.Core.Application.Dtos.PropertyType;
using RealStateApp.Core.Application.Dtos.SaleType;
using RealStateApp.Core.Application.ViewModels.PropertyType;
using RealStateApp.Core.Application.ViewModels.SaleType;

namespace RealStateApp.Core.Application.Mappings.DtosAndViewModels;

public class SaleTypeDtoMappingProfile : Profile
{
    public SaleTypeDtoMappingProfile()
    {
        CreateMap<SaleTypeDto, SaleTypeViewModel>().ReverseMap();
        CreateMap<SaleTypeWithCountDto, SaleTypeWithCountViewModel >().ReverseMap();
        CreateMap<CreateSaleTypeViewModel, SaleTypeDto>().ReverseMap();
        CreateMap<EditSaleTypeViewModel, SaleTypeDto>().ReverseMap();
    }
}
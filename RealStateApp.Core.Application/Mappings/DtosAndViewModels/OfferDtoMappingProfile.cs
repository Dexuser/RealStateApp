using AutoMapper;
using RealStateApp.Core.Application.Dtos.Offer;
using RealStateApp.Core.Application.ViewModels.Offer;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Mappings.DtosAndViewModels;

public class OfferDtoMappingProfile : Profile
{
    public OfferDtoMappingProfile()
    {
        CreateMap<OfferDto, OfferViewModel>().ReverseMap();
    }
}
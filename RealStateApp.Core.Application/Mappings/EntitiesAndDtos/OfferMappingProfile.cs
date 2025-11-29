using AutoMapper;
using RealStateApp.Core.Application.Dtos.Offer;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Mappings.EntitiesAndDtos;

public class OfferMappingProfile : Profile
{
    public OfferMappingProfile()
    {
        CreateMap<Offer, OfferDto>().ReverseMap();
    }
}
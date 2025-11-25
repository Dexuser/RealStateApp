using AutoMapper;
using RealStateApp.Core.Application.Dtos.Offer;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Services;

public class OfferService : GenericServices<Offer, OfferDto> , IOfferService
{
    public OfferService(IOfferRepository repository, IMapper mapper) : base(repository, mapper)
    {
    }
}
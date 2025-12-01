using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.Offer;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Services;

public class OfferService : GenericServices<Offer, OfferDto> , IOfferService
{
    private readonly IOfferRepository _offerRepository;
    private readonly IMapper _mapper;
    public OfferService(IOfferRepository repository, IMapper mapper) : base(repository, mapper)
    {
        _mapper = mapper;
        _offerRepository = repository;
    }

    public override Task<Result<OfferDto>> AddAsync(OfferDto dtoModel)
    {
        dtoModel.CreatedAt = DateTime.UtcNow;
        dtoModel.Status = OfferStatus.Pending;
        return base.AddAsync(dtoModel);
    }

    public async Task<List<OfferDto>> GetAllOffersOfThisClientOnThisProperty(string clientId, int propertyId)
    {
        var query = await _offerRepository.GetAllQueryable().AsQueryable()
            .Where(x => x.PropertyId == propertyId)
            .Where(x => x.UserId == clientId)
            .Select(x => _mapper.Map<OfferDto>(x))
            .ToListAsync();
        
        return query;
    }
}
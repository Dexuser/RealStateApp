using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.Offer;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Services;

public class OfferService : GenericServices<Offer, OfferDto> , IOfferService
{
    private readonly IOfferRepository _offerRepository;
    private readonly IMapper _mapper;
    private readonly IBaseAccountService _baseAccountService;
    private readonly IPropertyRepository _propertyRepository;
    public OfferService(IOfferRepository repository, IMapper mapper, IBaseAccountService baseAccountService, IPropertyRepository propertyRepository) : base(repository, mapper)
    {
        _mapper = mapper;
        _baseAccountService = baseAccountService;
        _propertyRepository = propertyRepository;
        _offerRepository = repository;
    }

    public override async Task<Result<OfferDto>> AddAsync(OfferDto dtoModel)
    {
        try
        {
            dtoModel.CreatedAt = DateTime.UtcNow;
            dtoModel.Status = OfferStatus.Pending;
            return await base.AddAsync(dtoModel);
        }
        catch (Exception ex)
        {
            return Result<OfferDto>.Fail(ex.Message);
        }
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
    

    public async Task<Result> RespondOffer(int offerId, bool acepted)
    {
        var offer =  await _offerRepository.GetByIdAsync(offerId);
        if (offer == null || offer.Status != OfferStatus.Pending)
        {
            return Result.Fail("Offer not found");
        }

        if (acepted)
        {
            offer.Status = OfferStatus.Accepted;
            await _offerRepository.UpdateAsync(offer.Id, offer);
            
            var property = await _propertyRepository.GetByIdAsync(offer.PropertyId);
            property!.IsAvailable = false;

            await _offerRepository.GetAllQueryable()
                .Where(x => x.PropertyId == offer.PropertyId 
                            && x.Status == OfferStatus.Pending 
                            && x.Id != offer.Id)
                .ExecuteUpdateAsync(update =>
                    update.SetProperty(o => o.Status, o => OfferStatus.Rejected)
                );

            
            await _propertyRepository.UpdateAsync(property.Id, property);
            
            return Result.Ok();
        }
        
        offer.Status = OfferStatus.Rejected;
        await _offerRepository.UpdateAsync(offer.Id, offer);
        return Result.Ok();
    }

    public async Task<List<UserDto>> GetAllUsersWhoHasOfferOnThisProperty(int propertyId)
    {
        var clientsIds = _offerRepository.GetAllQueryable().AsNoTracking()
            .Where(x => x.PropertyId == propertyId)
            .Select(x => x.UserId).ToList();
        
        var users = await _baseAccountService.GetUsersByIds(clientsIds);
        return users;
    }
}
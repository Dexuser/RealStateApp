using RealStateApp.Core.Application.Dtos.Offer;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Interfaces;

public interface IOfferService :  IGenericService<OfferDto>
{
    Task<List<OfferDto>> GetAllOffersOfThisClientOnThisProperty(string clientId, int propertyId);
    Task<Result> RespondOffer(int offerId, bool acepted);

    Task<List<UserDto>> GetAllUsersWhoHasOfferOnThisProperty(int propertyId);
}
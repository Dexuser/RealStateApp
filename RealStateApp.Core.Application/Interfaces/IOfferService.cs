using RealStateApp.Core.Application.Dtos.Offer;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Interfaces;

public interface IOfferService :  IGenericService<OfferDto>
{
    Task<List<OfferDto>> GetAllOffersOfThisClientOnThisProperty(string clientId, int propertyId);
}
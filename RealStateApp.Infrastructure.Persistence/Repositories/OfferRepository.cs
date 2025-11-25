using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;
using RealStateApp.Infrastructure.Persistence.Contexts;

namespace RealStateApp.Infrastructure.Persistence.Repositories;

public class OfferRepository : GenericRepository<Offer>, IOfferRepository
{
    public OfferRepository(RealStateAppContext context) : base(context)
    {
    }
}
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;
using RealStateApp.Infrastructure.Persistence.Contexts;

namespace RealStateApp.Infrastructure.Persistence.Repositories;

public class FavoritePropertyRepository : GenericRepository<FavoriteProperty>, IFavoritePropertyRepository
{
    public FavoritePropertyRepository(RealStateAppContext context) : base(context)
    {
    }
}
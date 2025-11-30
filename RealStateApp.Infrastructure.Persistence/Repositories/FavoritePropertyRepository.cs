using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;
using RealStateApp.Infrastructure.Persistence.Contexts;

namespace RealStateApp.Infrastructure.Persistence.Repositories;

public class FavoritePropertyRepository : GenericRepository<FavoriteProperty>, IFavoritePropertyRepository
{
    public FavoritePropertyRepository(RealStateAppContext context) : base(context)
    {
    }

    public async Task ToggleFavoriteAsync(int propertyId, string userId)
    {
        var row = Context.Set<FavoriteProperty>().FirstOrDefault(x => x.PropertyId == propertyId && x.UserId == userId);
        if (row == null)
        {
            await AddAsync(new FavoriteProperty { PropertyId = propertyId, UserId = userId, Id = 0 });
        }
        else
        {
            Context.Set<FavoriteProperty>().Remove(row);
            await Context.SaveChangesAsync();
        }
    }
}
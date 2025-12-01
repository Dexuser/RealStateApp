using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Domain.Interfaces;

public interface IFavoritePropertyRepository : IGenericRepository<FavoriteProperty>
{
    Task ToggleFavoriteAsync(int propertyId, string userId);
}
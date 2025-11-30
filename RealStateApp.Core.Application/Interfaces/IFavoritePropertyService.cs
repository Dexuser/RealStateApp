using RealStateApp.Core.Application.Dtos.FavoriteProperty;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Interfaces;

public interface IFavoritePropertyService : IGenericService<FavoritePropertyDto>
{
    Task ToggleFavoriteAsync(int propertyId, string userId);
}
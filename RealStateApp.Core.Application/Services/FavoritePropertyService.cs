using AutoMapper;
using RealStateApp.Core.Application.Dtos.FavoriteProperty;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Services;

public class FavoritePropertyService : GenericServices<FavoriteProperty, FavoritePropertyDto>, IFavoritePropertyService
{
    private readonly IFavoritePropertyRepository _favoritePropertyRepository; 
    public FavoritePropertyService(IFavoritePropertyRepository repository, IMapper mapper, IFavoritePropertyRepository favoritePropertyRepository) : base(repository, mapper)
    {
        _favoritePropertyRepository = favoritePropertyRepository;
    }

    public async Task ToggleFavoriteAsync(int propertyId, string userId)
    {
        await _favoritePropertyRepository.ToggleFavoriteAsync(propertyId, userId);
    }
}
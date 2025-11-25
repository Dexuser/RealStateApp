using AutoMapper;
using RealStateApp.Core.Application.Dtos.FavoriteProperty;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Services;

public class FavoritePropertyService : GenericServices<FavoriteProperty, FavoritePropertyDto>, IFavoritePropertyService
{
    public FavoritePropertyService(IFavoritePropertyRepository repository, IMapper mapper) : base(repository, mapper)
    {
    }
}
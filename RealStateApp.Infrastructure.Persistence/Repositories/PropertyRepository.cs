using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;
using RealStateApp.Infrastructure.Persistence.Contexts;

namespace RealStateApp.Infrastructure.Persistence.Repositories;

public class PropertyRepository :  GenericRepository<Property>, IPropertyRepository
{
    public PropertyRepository(RealStateAppContext context) : base(context)
    {
    }
}
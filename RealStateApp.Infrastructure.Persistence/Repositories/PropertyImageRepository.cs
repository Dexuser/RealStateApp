using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;
using RealStateApp.Infrastructure.Persistence.Contexts;

namespace RealStateApp.Infrastructure.Persistence.Repositories;

public class PropertyImageRepository : GenericRepository<PropertyImage>, IPropertyImageRepository
{
    public PropertyImageRepository(RealStateAppContext context) : base(context)
    {
    }
}
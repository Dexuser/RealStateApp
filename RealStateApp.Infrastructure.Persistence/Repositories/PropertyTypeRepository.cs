using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;
using RealStateApp.Infrastructure.Persistence.Contexts;

namespace RealStateApp.Infrastructure.Persistence.Repositories;

public class PropertyTypeRepository :  GenericRepository<PropertyType>, IPropertyTypeRepository
{
    public PropertyTypeRepository(RealStateAppContext context) : base(context)
    {
    }
}
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;
using RealStateApp.Infrastructure.Persistence.Contexts;

namespace RealStateApp.Infrastructure.Persistence.Repositories;

public class PropertyImprovementRepository : GenericRepository<PropertyImprovement> , IPropertyImprovementRepository
{
    public PropertyImprovementRepository(RealStateAppContext context) : base(context)
    {
    }
}
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;
using RealStateApp.Infrastructure.Persistence.Contexts;

namespace RealStateApp.Infrastructure.Persistence.Repositories;

public class ImprovementRepository :  GenericRepository<Improvement>, IImprovementRepository
{
    public ImprovementRepository(RealStateAppContext context) : base(context)
    {
    }
}
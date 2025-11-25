using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;
using RealStateApp.Infrastructure.Persistence.Contexts;

namespace RealStateApp.Infrastructure.Persistence.Repositories;

public class SaleTypeRepository : GenericRepository<SaleType>, ISaleTypeRepository
{
    public SaleTypeRepository(RealStateAppContext context) : base(context)
    {
    }
}
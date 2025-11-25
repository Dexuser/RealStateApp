using AutoMapper;
using RealStateApp.Core.Application.Dtos.PropertyImprovement;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Services;

public class PropertyImprovementService : GenericServices<PropertyImprovement, PropertyImprovementDto>, IPropertyImprovementService
{
    public PropertyImprovementService(IPropertyImprovementRepository repository, IMapper mapper) : base(repository, mapper)
    {
    }
}
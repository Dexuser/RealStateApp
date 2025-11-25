using AutoMapper;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Services;

public class PropertyService : GenericServices<Property, PropertyDto>, IPropertyService
{
    public PropertyService(IPropertyRepository repository, IMapper mapper) : base(repository, mapper)
    {
    }
}
using AutoMapper;
using RealStateApp.Core.Application.Dtos.PropertyType;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Services;

public class PropertyTypeService :  GenericServices<PropertyType, PropertyTypeDto>, IPropertyTypeService
{
    public PropertyTypeService(IPropertyTypeRepository repository, IMapper mapper) : base(repository, mapper)
    {
    }
}
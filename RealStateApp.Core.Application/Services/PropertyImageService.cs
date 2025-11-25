using AutoMapper;
using RealStateApp.Core.Application.Dtos.PropertyImage;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Services;

public class PropertyImageService :  GenericServices<PropertyImage, PropertyImageDto>,  IPropertyImageService
{
    public PropertyImageService(IPropertyImageRepository repository, IMapper mapper) : base(repository, mapper)
    {
    }
}
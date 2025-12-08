using RealStateApp.Core.Application.Dtos.PropertyImage;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Interfaces;

public interface IPropertyImageService :  IGenericService<PropertyImageDto>
{
    Task<List<PropertyImageDto>> GetAllImagesOfThisProperty(int propertyId);
    Task<Result> DeleteAllAdditionalImagesOfThisPropertyAsync(int propertyId);
}
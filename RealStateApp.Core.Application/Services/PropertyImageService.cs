using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.PropertyImage;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Services;

public class PropertyImageService :  GenericServices<PropertyImage, PropertyImageDto>,  IPropertyImageService
{
    private readonly IPropertyImageRepository _propertyImageRepository;
    public PropertyImageService(IPropertyImageRepository repository, IMapper mapper) : base(repository, mapper)
    {
        _propertyImageRepository = repository;
    }
    
    public async Task<Result> DeleteAllAdditionalImagesOfThisPropertyAsync(int propertyId)
    {
        try
        {
            await _propertyImageRepository.GetAllQueryable().Where(p => p.PropertyId == propertyId && !p.IsMain)
                .ExecuteDeleteAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail("an error has occurred while trying to delete the additional images of the property");
        }
    }

    public async Task<List<PropertyImageDto>> GetAllImagesOfThisProperty(int propertyId)
    {
        var images = await _propertyImageRepository.GetAllQueryable().Where(p => p.PropertyId == propertyId)
            .Select(img => new  PropertyImageDto
            {
                Id = img.Id,
                ImagePath = img.ImagePath,
                PropertyId = img.PropertyId,
                IsMain = img.IsMain 
            })
            .ToListAsync();

        return images;
    }
}
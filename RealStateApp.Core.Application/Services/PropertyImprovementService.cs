using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.PropertyImprovement;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Services;

public class PropertyImprovementService: GenericServices<PropertyImprovement, PropertyImprovementDto>, IPropertyImprovementService
{
    private IPropertyImprovementRepository _propertyImprovementRepository;
    private readonly IImprovementRepository _improvementRepository;
    private readonly IMapper _mapper;


    public PropertyImprovementService(IPropertyImprovementRepository propertyImprovementRepository, IMapper mapper, IImprovementRepository improvementRepository) : base(propertyImprovementRepository, mapper)
    {
        _propertyImprovementRepository = propertyImprovementRepository;
        _improvementRepository = improvementRepository;
        _mapper = mapper;
    }

    public async Task<List<SelectListItem>> GetSelectListAsync()
    {
        var type = await _improvementRepository.GetAllAsync();

        return type
            .Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name
            }).ToList();
    }

    public async Task<Result> DeleteAllImprovementsOfAPropertyAsync(int propertyId)
    {
        try
        {
            await _propertyImprovementRepository.GetAllQueryable().Where(pi => pi.PropertyId == propertyId).ExecuteDeleteAsync();
            return Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Fail("An error ocurred while trying to delete all improvements of a property");
        }

    }
}
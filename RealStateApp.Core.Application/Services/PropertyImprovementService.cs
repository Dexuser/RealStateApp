using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using RealStateApp.Core.Application.Dtos.PropertyImprovement;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Services;

public class PropertyImprovementService(
    IPropertyImprovementRepository repository,
    IMapper mapper,
    IImprovementRepository improvementRepository)
    : GenericServices<PropertyImprovement, PropertyImprovementDto>(repository, mapper), IPropertyImprovementService
{
    private readonly IImprovementRepository _improvementRepository = improvementRepository;

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
}
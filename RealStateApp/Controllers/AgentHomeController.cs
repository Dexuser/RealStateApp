using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.ViewModels.Property;
using RealStateApp.Core.Application.ViewModels.PropertyType;
using RealStateApp.Core.Application.ViewModels.User;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Controllers;

public class AgentHomeController : Controller
{
    private readonly IAccountServiceForWebApp _accountServiceForWebApp;
    private readonly IPropertyTypeService _propertyTypeService;
    private readonly IPropertyService _propertyService;
    private readonly IAgentService _agentService;
    private readonly IMapper _mapper;
    // GET
    public AgentHomeController(IAgentService agentService, IMapper mapper, IPropertyService propertyService, IPropertyTypeService propertyTypeService, IAccountServiceForWebApp accountServiceForWebApp)
    {
        _agentService = agentService;
        _mapper = mapper;
        _propertyService = propertyService;
        _propertyTypeService = propertyTypeService;
        _accountServiceForWebApp = accountServiceForWebApp;
    }

    public async Task<IActionResult> Index(string? name = null)
    {
        var agents = _mapper.Map<List<UserViewModel>>(await _agentService.GetAllAgents(true, name));
        return View(agents);
    }

    public async Task<IActionResult> Properties(PropertyViewModelFilters filters)
    {
        
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        var filtersDto = new PropertyFiltersDto
        {
            AgentId = filters.AgentId,
            SelectedPropertyTypeId = filters.SelectedPropertyTypeId,
            MinValue = filters.MinValue,
            MaxValue = filters.MaxValue,
            Bathrooms = filters.Bathrooms,
            Rooms = filters.Rooms,
            ClientId = userId,
            OnlyFavorites = false,
            PropertyCode = filters.PropertyCode
        };
        
        var model = new AgentPropertiesViewModel()
        {
            Agent = _mapper.Map<UserViewModel>(await _accountServiceForWebApp.GetUserById(filters.AgentId!)),
            Properties = _mapper.Map<List<PropertyViewModel>>(await _propertyService.GetAllAvailablePropertiesAsync(filtersDto)),
            PropertyTypes = _mapper.Map<List<PropertyTypeViewModel>>(await _propertyTypeService.GetAllAsync()),
            Filters = filters, 
        };

        return View(model);
    }
    
    public async Task<IActionResult> Details(int id)
    {
        var model = _mapper.Map<PropertyViewModel>(await _propertyService.GetByIdAsync(id));
        return View(model);
    }
}
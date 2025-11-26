using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Application.ViewModels.DashBoard;
using RealStateApp.Core.Domain.Common;

namespace RealStateApp.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{nameof(Roles.Admin)}")] 
public class HomeController : Controller
{
    private readonly IDashBoardService _dashBoardService;
    private readonly IMapper _mapper;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, IDashBoardService dashBoardService, IMapper mapper)
    {
        _logger = logger;
        _dashBoardService = dashBoardService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var model = _mapper.Map<AdminDashBoardViewModel>(await _dashBoardService.GetAdminDashBoard());
        return View(model);
    }
}
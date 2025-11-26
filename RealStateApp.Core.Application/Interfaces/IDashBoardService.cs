using RealStateApp.Core.Application.Dtos.DashBoard;

namespace RealStateApp.Core.Application.Services;

public interface IDashBoardService
{
    Task<AdminDashBoardDto> GetAdminDashBoard();
}
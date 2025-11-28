using RealStateApp.Core.Application.Dtos.DashBoard;

namespace RealStateApp.Core.Application.Interfaces;

public interface IDashBoardService
{
    Task<AdminDashBoardDto> GetAdminDashBoard();
}
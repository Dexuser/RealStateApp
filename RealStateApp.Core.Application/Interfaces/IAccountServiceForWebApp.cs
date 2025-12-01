using RealStateApp.Core.Application.Dtos.Login;
using RealStateApp.Core.Application.Dtos.User;

namespace RealStateApp.Core.Application.Interfaces;

public interface IAccountServiceForWebApp : IBaseAccountService
{
    Task<Result<UserDto>> AuthenticateAsync(LoginDto loginDto);
    Task SignOutAsync();
}
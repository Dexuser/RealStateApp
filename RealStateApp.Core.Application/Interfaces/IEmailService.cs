using RealStateApp.Core.Application.Dtos.Email;

namespace RealStateApp.Core.Application.Interfaces;

public interface IEmailService
{
    Task SendAsync(EmailRequestDto emailRequestDto);
}
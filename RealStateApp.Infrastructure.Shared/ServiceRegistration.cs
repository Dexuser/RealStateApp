using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Settings;
using RealStateApp.Infrastructure.Shared.Services;

namespace RealStateApp.Infrastructure.Shared;

public static class ServiceRegistration
{
    public static void AddSharedLayerIoc(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<MailSettings>(config.GetSection("MailSettings"));
        services.AddScoped<IEmailService, EmailService>();
   }
}
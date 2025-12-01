using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealStateApp.Core.Application.Behaviors;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application
{
    public static class ServiceRegistration
    {
        //Extension method - Decorator pattern
        public static void AddApplicationLayerIoc(this IServiceCollection services, IConfiguration config)
        {
            // Configurations
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(opt=> opt.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            
            // Services
            services.AddScoped<IChatMessageService, ChatMessageService>();
            services.AddScoped<IFavoritePropertyService, FavoritePropertyService>();
            services.AddScoped<IImprovementService, ImprovementService>();
            services.AddScoped<IOfferService, OfferService>();
            services.AddScoped<IPropertyService, PropertyService>();
            services.AddScoped<IPropertyImageService, PropertyImageService>();
            services.AddScoped<IPropertyImprovementService, PropertyImprovementService>();
            services.AddScoped<IPropertyTypeService, PropertyTypeService>();
            services.AddScoped<ISaleTypeService, SaleTypeService>();
            
            services.AddScoped<IAgentService, AgentService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IDeveloperService, DeveloperService>();
            services.AddScoped<IDashBoardService, DashBoardService>();
            
        }
    }
}

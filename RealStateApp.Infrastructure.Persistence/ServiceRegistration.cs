using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealStateApp.Core.Domain.Interfaces;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Infrastructure.Persistence;

public static class ServiceRegistration
{
    public static void AddPersistenceLayerIoc(this IServiceCollection services, IConfiguration config)
    {
        if (config.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<RealStateAppContext>(options => options.UseInMemoryDatabase("AppDb"));
        }
        else
        {
            services.AddDbContext<RealStateAppContext>(
                opt =>
                {
                    opt.EnableSensitiveDataLogging();
                    opt.UseSqlServer(
                        config.GetConnectionString("DefaultConnection"),
                        m => m.MigrationsAssembly(typeof(RealStateAppContext).Assembly.FullName)
                    );
                } 
            );
        
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
            services.AddScoped<IFavoritePropertyRepository, FavoritePropertyRepository>();
            services.AddScoped<IImprovementRepository, ImprovementRepository>();
            services.AddScoped<IOfferRepository, OfferRepository>();
            services.AddScoped<IPropertyRepository, PropertyRepository >();
            services.AddScoped<IPropertyImageRepository, PropertyImageRepository>();
            services.AddScoped<IPropertyImprovementRepository, PropertyImprovementRepository>();
            services.AddScoped<IPropertyTypeRepository, PropertyTypeRepository>();
            services.AddScoped<ISaleTypeRepository, SaleTypeRepository>();           
        }
    }
}
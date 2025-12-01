using Asp.Versioning;
using Microsoft.OpenApi;

namespace RealStateApi.Extensions
{
    public static class ServiceExtension
    {
        // Swagger como tal
        public static void AddSwaggerExtension(this IServiceCollection services) {

            services.AddSwaggerGen(options =>
            {
                // Revisa toda la documentacion del proyecto y lo pone en Swagger
                List<string> xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", searchOption: SearchOption.TopDirectoryOnly).ToList();
                xmlFiles.ForEach(xmlFile => options.IncludeXmlComments(xmlFile));
                
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1.0",
                    Title = "RealStateApp API",
                    Description = "This Api will be responsible for overall data distribution",
                    Contact = new OpenApiContact
                    {
                        Name = "Luis, Albin",
                        Email = "urenaperdomo@gmail.com",                 
                    }
                });


                options.DescribeAllParametersInCamelCase();
                options.EnableAnnotations();
                
                options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("bearer", document)] = []
                });

            });        
        }

        // Versionamiento de la API
        public static void AddAppiVersioningExtension(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true; // reporta el status de la version
                opt.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("X-Api-Version")
                );
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV"; // ← Debe quedar así (con comillas simples)
                options.SubstituteApiVersionInUrl = true;
            });
        }
    }
}

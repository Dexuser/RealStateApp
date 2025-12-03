using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using RealStateApi.Extensions;
using RealStateApi.Handlers;
using RealStateApp.Core.Application;
using RealStateApp.Infrastructure.Identity;
using RealStateApp.Infrastructure.Persistence;
using RealStateApp.Infrastructure.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers(opt =>
    {
        opt.Filters.Add(new ProducesAttribute("application/json"));
    })
    .ConfigureApiBehaviorOptions(opt =>
    {
        opt.SuppressInferBindingSourcesForParameters = true; // Tienes que poner [FromRoute] o [FromBody] obligado
        opt.SuppressMapClientErrors = true;
    })
    .AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});;
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

// Registrar capas de la aplicación
builder.Services.AddApplicationLayerIoc(builder.Configuration);
builder.Services.AddPersistenceLayerIoc(builder.Configuration);
builder.Services.AddIdentityLayerIocForWebApi(builder.Configuration);
builder.Services.AddSharedLayerIoc(builder.Configuration);

builder.Services.AddEndpointsApiExplorer(); // Ayuda configurar metada para documentacion (para swagger en realidad)
builder.Services.AddHealthChecks(); // Diagnostica el estado de salud
// esto construye swagger
builder.Services.AddAppiVersioningExtension(); // Metodo de extension
builder.Services.AddSwaggerExtension(); // metodo de extension

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// CORS configuration (si es necesario para desarrollo)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();
await app.Services.RunIdentitySeedAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerExtension(app);
    app.UseCors("AllowAll");
}

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseHealthChecks("/health"); // Una URL ara consultar la salud de la API

app.MapControllers();

app.Run();
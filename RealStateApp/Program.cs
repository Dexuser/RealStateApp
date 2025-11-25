using RealStateApp.Core.Application;
using RealStateApp.Handlers;
using RealStateApp.Infrastructure.Identity;
using RealStateApp.Infrastructure.Persistence;
using RealStateApp.Infrastructure.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddApplicationLayerIoc(builder.Configuration);
builder.Services.AddPersistenceLayerIoc(builder.Configuration);
builder.Services.AddIdentityLayerIocForWebApp(builder.Configuration);
builder.Services.AddSharedLayerIoc(builder.Configuration);
builder.Services.AddSession();

//builder.Services.AddExceptionHandler<WebAppExceptionHandler>();
//builder.Services.AddProblemDetails();

var app = builder.Build();
await app.Services.RunIdentitySeedAsync();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
//app.UseExceptionHandler();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
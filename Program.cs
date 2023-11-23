using Microsoft.AspNetCore.SpaServices.AngularCli;
using MintPlayer.AspNetCore.Hsts;
using MintPlayer.AspNetCore.SpaServices.Prerendering;
using MintPlayer.AspNetCore.SpaServices.Routing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// In production, the Angular files will be served from this directory
builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "ClientApp/dist";
});

builder.Services.AddSpaPrerenderingService<Universal2024.Services.SpaPrerenderingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseImprovedHsts();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

if (!builder.Environment.IsDevelopment())
{
    app.UseSpaStaticFiles();
}

app.UseSpa(spa =>
{
    spa.Options.SourcePath = "ClientApp";
    spa.UseSpaPrerendering(options =>
    {
        //options.BootModuleBuilder = builder.Environment.IsDevelopment() ? new AngularPrerendererBuilder(npmScript: "build:ssr") : null;
        options.BootModulePath = $"{spa.Options.SourcePath}/dist/server/main.js";
        options.ExcludeUrls = new[] { "/sockjs-node" };
    });

    if (builder.Environment.IsDevelopment())
    {
        spa.UseAngularCliServer(npmScript: "start");
    }
});

app.Run();
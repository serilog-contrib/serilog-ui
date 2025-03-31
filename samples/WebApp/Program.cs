using Serilog;
using Serilog.Ui.Web.Extensions;
using Serilog.Ui.Web.Models;
using WebApp.Extensions;
using WebApp.HostedServices;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
    loggerConfiguration
        .ReadFrom.Configuration(hostingContext.Configuration)
        .Enrich.FromLogContext());

// Add services to the container.
builder.Services
    .AddHostedService<MongoDbService>()
    .AddAuthenticationDetails(builder.Configuration)
    .AddSerilogUiSample(builder.Configuration)
    .AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

var serverSubPath = bool.Parse(builder.Configuration["SerilogUi:AddServerSubPath"] ?? "false") ? "logs/" : "";
app.UseSerilogUi(options => options
    .WithHomeUrl("/#Test")
    .WithServerSubPath(serverSubPath)
    .WithAuthenticationType(AuthenticationType.Jwt)
    .WithExpandedDropdownsByDefault()
    .EnableAuthorizationOnAppRoutes()
    .InjectJavascript($"/{serverSubPath}js/serilog-ui/custom.js")
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
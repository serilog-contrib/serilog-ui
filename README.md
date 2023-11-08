# serilog-ui ![GitHub release (latest by date)](https://img.shields.io/github/v/release/serilog-contrib/serilog-ui?label=latest%20release) [![](https://img.shields.io/nuget/dt/serilog.ui.svg?label=nuget%20downloads)](Serilog.UI)

[![DotNET-build](https://github.com/serilog-contrib/serilog-ui/actions/workflows/DotNET-build.yml/badge.svg?branch=master)](https://github.com/serilog-contrib/serilog-ui/actions/workflows/DotNET-build.yml)
[![DotNET Coverage](https://sonarcloud.io/api/project_badges/measure?project=followynne_serilog-ui&metric=coverage)](https://sonarcloud.io/summary/new_code?id=followynne_serilog-ui)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=followynne_serilog-ui&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=followynne_serilog-ui)

[![JS-build](https://github.com/serilog-contrib/serilog-ui/actions/workflows/JS-build.yml/badge.svg?branch=master)](https://github.com/serilog-contrib/serilog-ui/actions/workflows/JS-build.yml)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=followynne_serilog-ui_assets&metric=coverage)](https://sonarcloud.io/summary/new_code?id=followynne_serilog-ui_assets)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=followynne_serilog-ui_assets&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=followynne_serilog-ui_assets)

A simple Serilog log viewer for the following sinks:

- Serilog.Sinks.**MSSqlServer** ([Nuget](https://github.com/serilog/serilog-sinks-mssqlserver))
- Serilog.Sinks.**MySql** ([Nuget](https://github.com/TeleSoftas/serilog-sinks-mariadb))
- Serilog.Sinks.**Postgresql** ([Nuget](https://github.com/b00ted/serilog-sinks-postgresql))
- Serilog.Sinks.**MongoDB** ([Nuget](https://github.com/serilog/serilog-sinks-mongodb))
- Serilog.Sinks.**ElasticSearch** ([Nuget](https://github.com/serilog/serilog-sinks-elasticsearch))

<img src="https://raw.githubusercontent.com/mo-esmp/serilog-ui/master/assets/serilog-ui.jpg" width="100%" />


# Read the [Wiki :blue_book:](https://github.com/serilog-contrib/serilog-ui/wiki)

## Quick Start :dash:

### Nuget packages installation

Install the _Serilog.UI_ [NuGet package](https://www.nuget.org/packages/Serilog.UI):

```powershell
# using dotnet cli
dotnet add package Serilog.UI

# using package manager:
Install-Package Serilog.UI
```

Install one of the available providers, based upon your sink:

| Provider | install: dotnet | install: pkg manager |
| --- | --- | --- |
| **Serilog.UI.MsSqlServerProvider** [[NuGet](https://www.nuget.org/packages/Serilog.UI.MsSqlServerProvider)] | `dotnet add package Serilog.UI.MsSqlServerProvider` | `Install-Package Serilog.UI.MsSqlServerProvider` |
| **Serilog.UI.MySqlProvider** [[NuGet](https://www.nuget.org/packages/Serilog.UI.MySqlProvider)] | `dotnet add package Serilog.UI.MySqlProvider` | `Install-Package Serilog.UI.MySqlProvider` |
| **Serilog.UI.PostgreSqlProvider** [[NuGet](https://www.nuget.org/packages/Serilog.UI.PostgreSqlProvider)] | `dotnet add package Serilog.UI.PostgreSqlProvider` | `Install-Package Serilog.UI.PostgreSqlProvider` |
| **Serilog.UI.MongoDbProvider** [[NuGet](https://www.nuget.org/packages/Serilog.UI.MongoDbProvider)] | `dotnet add package Serilog.UI.MongoDbProvider` | `Install-Package Serilog.UI.MongoDbProvider` |
| **Serilog.UI.ElasticSearchProvider** [[NuGet](https://www.nuget.org/packages/Serilog.UI.ElasticSearchProvider)] | `dotnet add package Serilog.UI.ElasticSearchProvider` | `Install-Package Serilog.UI.ElasticSearcProvider` |

### DI registration

Add `AddSerilogUi()` to `IServiceCollection` in your `Startup.ConfigureServices` method:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Register the serilog UI services
    services.AddSerilogUi(options => 
      // each provider exposes extension methods to configure.
      // example with MSSqlServerProvider:
      options.UseSqlServer("ConnectionString", "LogTableName"));
}
```

In the `Startup.Configure` method, enable the middleware to serve the log UI page. 
Note: call to the `UseSerilogUi` middleware must be placed **_after_** any Authentication and Authorization middleware, otherwise the authentication may not work:

```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    (...)

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
        
    // Enable middleware to serve log-ui (HTML, JS, CSS, etc.).
    app.UseSerilogUi();

    (...)

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
    });
}
```

## Basic Authentication

If you need to add basic authentication to your serilog-ui instance, you can use the `BasicAuthenticationFilter`. Here's how to configure it in your `Startup.Configure` method:

```csharp
app.UseSerilogUi(options =>
{
    options.Authorization.Filters = new IUiAuthorizationFilter[]
    {
        new BasicAuthenticationFilter { User = "User", Pass = "P@ss" }
    };
    options.Authorization.RunAuthorizationFilterOnAppRoutes = true;
});
```

### For further configuration: [:fast_forward:](https://github.com/serilog-contrib/serilog-ui/wiki/Install:-Configuration-Options)

## Running the Tests: [:test_tube:](https://github.com/serilog-contrib/serilog-ui/wiki/Development:-Testing)

## License

See [LICENSE](https://github.com/serilog-contrib/serilog-ui/blob/master/LICENSE).

## Issues and Contribution

Everything is welcome! :trophy: See the [contribution guidelines](https://github.com/serilog-contrib/serilog-ui/blob/master/CONTRIBUTING.md) for details.

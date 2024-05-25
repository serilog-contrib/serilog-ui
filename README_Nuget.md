# serilog-ui

A simple Serilog log viewer for the following sinks:

- Serilog.Sinks.**MSSqlServer** ([Nuget](https://github.com/serilog/serilog-sinks-mssqlserver))
- Serilog.Sinks.**MySql** ([Nuget](https://github.com/TeleSoftas/serilog-sinks-mariadb)) and Serilog.Sinks.**MariaDB
  ** [Nuget](https://github.com/TeleSoftas/serilog-sinks-mariadb)
- Serilog.Sinks.**Postgresql** ([Nuget](https://github.com/b00ted/serilog-sinks-postgresql)) and Serilog.Sinks.*
  *Postgresql.Alternative** ([Nuget](https://github.com/serilog-contrib/Serilog.Sinks.Postgresql.Alternative))
- Serilog.Sinks.**MongoDB** ([Nuget](https://github.com/serilog/serilog-sinks-mongodb))
- Serilog.Sinks.**ElasticSearch** ([Nuget](https://github.com/serilog/serilog-sinks-elasticsearch))
- Serilog.Sinks.**RavenDB** ([Nuget](https://github.com/ravendb/serilog-sinks-ravendb))

# Read the [Wiki](https://github.com/serilog-contrib/serilog-ui/wiki)

## Quick Start

### Nuget packages installation

Install the _Serilog.UI_ [NuGet package](https://www.nuget.org/packages/Serilog.UI):

```powershell
# using dotnet cli
dotnet add package Serilog.UI

# using package manager:
Install-Package Serilog.UI
```

Install one or more of the available providers, based upon your sink(s):

| Provider                                                                                                        | install: dotnet                                       | install: pkg manager                               |
| --------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------- | -------------------------------------------------- |
| **Serilog.UI.MsSqlServerProvider** [[NuGet](https://www.nuget.org/packages/Serilog.UI.MsSqlServerProvider)]     | `dotnet add package Serilog.UI.MsSqlServerProvider`   | `Install-Package Serilog.UI.MsSqlServerProvider`   |
| **Serilog.UI.MySqlProvider** [[NuGet](https://www.nuget.org/packages/Serilog.UI.MySqlProvider)]                 | `dotnet add package Serilog.UI.MySqlProvider`         | `Install-Package Serilog.UI.MySqlProvider`         |
| **Serilog.UI.PostgreSqlProvider** [[NuGet](https://www.nuget.org/packages/Serilog.UI.PostgreSqlProvider)]       | `dotnet add package Serilog.UI.PostgreSqlProvider`    | `Install-Package Serilog.UI.PostgreSqlProvider`    |
| **Serilog.UI.MongoDbProvider** [[NuGet](https://www.nuget.org/packages/Serilog.UI.MongoDbProvider)]             | `dotnet add package Serilog.UI.MongoDbProvider`       | `Install-Package Serilog.UI.MongoDbProvider`       |
| **Serilog.UI.ElasticSearchProvider** [[NuGet](https://www.nuget.org/packages/Serilog.UI.ElasticSearchProvider)] | `dotnet add package Serilog.UI.ElasticSearchProvider` | `Install-Package Serilog.UI.ElasticSearchProvider` |
| **Serilog.UI.RavenDbProvider** [[NuGet](https://www.nuget.org/packages/Serilog.UI.RavenDbProvider)]             | `dotnet add package Serilog.UI.RavenDbProvider`       | `Install-Package Serilog.UI.RavenDbProvider`       |

### DI registration

Add `AddSerilogUi()` to `IServiceCollection` in your `Startup.ConfigureServices` method:

```csharp
public void ConfigureServices(IServiceCollection services)
{
  // Register the serilog UI services
  services.AddSerilogUi(options => options// each provider exposes extension methods to configure.
    // example with MSSqlServerProvider:
    .UseSqlServer(opts => opts
      .WithConnectionString("YOUR_CONNECTION_STRING")
      .WithTable("YOUR_TABLE")));
}
```

In the `Startup.Configure` method or on the WebApplication builder, enable the middleware to serve the log UI page.

NOTE: call to the `UseSerilogUi` middleware must be placed **_after_** any Authentication and Authorization middleware!

```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    (...)

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    // Enable middleware to serve log-ui (HTML, JS, CSS, etc.).
    app.UseSerilogUi(opts => [...]);

    (...)
}
```

### [For further configuration](https://github.com/serilog-contrib/serilog-ui/wiki/Configure)

## Issues and Contribution

Everything is welcome! See
the [contribution guidelines](https://github.com/serilog-contrib/serilog-ui/blob/master/CONTRIBUTING.md) for details.

For details on running the project, start reading from [Develop](https://github.com/serilog-contrib/serilog-ui/wiki/Develop).

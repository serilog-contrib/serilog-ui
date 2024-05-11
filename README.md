# serilog-ui ![GitHub release (latest by date)](https://img.shields.io/github/v/release/serilog-contrib/serilog-ui?label=latest%20release) [![](https://img.shields.io/nuget/dt/serilog.ui.svg?label=nuget%20downloads)](Serilog.UI)

[![DotNET-build](https://github.com/serilog-contrib/serilog-ui/actions/workflows/DotNET-build.yml/badge.svg?branch=master)](https://github.com/serilog-contrib/serilog-ui/actions/workflows/DotNET-build.yml)
[![DotNET Coverage](https://sonarcloud.io/api/project_badges/measure?project=followynne_serilog-ui&metric=coverage)](https://sonarcloud.io/summary/new_code?id=followynne_serilog-ui)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=followynne_serilog-ui&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=followynne_serilog-ui)

[![JS-build](https://github.com/serilog-contrib/serilog-ui/actions/workflows/JS-build.yml/badge.svg?branch=master)](https://github.com/serilog-contrib/serilog-ui/actions/workflows/JS-build.yml)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=followynne_serilog-ui_assets&metric=coverage)](https://sonarcloud.io/summary/new_code?id=followynne_serilog-ui_assets)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=followynne_serilog-ui_assets&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=followynne_serilog-ui_assets)

A simple Serilog log viewer for the following sinks:

- Serilog.Sinks.**MSSqlServer** ([Nuget](https://github.com/serilog/serilog-sinks-mssqlserver))
- Serilog.Sinks.**MySql** ([Nuget](https://github.com/saleem-mirza/serilog-sinks-mysql)) and Serilog.Sinks.**MariaDB** [Nuget](https://github.com/TeleSoftas/serilog-sinks-mariadb)
- Serilog.Sinks.**Postgresql** ([Nuget](https://github.com/b00ted/serilog-sinks-postgresql)) and Serilog.Sinks.\*
  \*Postgresql.Alternative\*\* ([Nuget](https://github.com/serilog-contrib/Serilog.Sinks.Postgresql.Alternative))
- Serilog.Sinks.**MongoDB** ([Nuget](https://github.com/serilog/serilog-sinks-mongodb))
- Serilog.Sinks.**ElasticSearch** ([Nuget](https://github.com/serilog/serilog-sinks-elasticsearch))
- Serilog.Sinks.**RavenDB** ([Nuget](https://github.com/ravendb/serilog-sinks-ravendb))

<img src="https://raw.githubusercontent.com/mo-esmp/serilog-ui/master/assets/serilog-ui-v3.jpg" width="100%" />

# Read the [Wiki :blue_book:](https://github.com/serilog-contrib/serilog-ui/wiki)

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

### For further configuration: [:fast_forward:](https://github.com/serilog-contrib/serilog-ui/wiki/Configure)

Do you want to test the package on-the-fly? Try out the [samples](https://github.com/serilog-contrib/serilog-ui/wiki/Develop#sample-applications), with no configuration required!

## Issues and Contribution

Everything is welcome! :trophy: See the [contribution guidelines](https://github.com/serilog-contrib/serilog-ui/blob/master/CONTRIBUTING.md) for details.

For details on running the project, start reading from [Develop](https://github.com/serilog-contrib/serilog-ui/wiki/Develop).

## License

See [LICENSE](https://github.com/serilog-contrib/serilog-ui/blob/master/LICENSE).

## Project Sponsors

<a href="https://www.jetbrains.com/" target="_blank"><img src="https://resources.jetbrains.com/storage/products/company/brand/logos/jb_beam.svg" style="width:120px"/></a>

## Contributors

<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->
<table>
  <tbody>
    <tr>
      <td align="center" valign="top" width="10%"><a href="https://stackoverflow.com/users/1385614/mohsen-esmailpour"><img src="https://avatars.githubusercontent.com/u/1659032?v=4?s=60" width="60px;" alt="Mohsen Esmailpour"/><br /><sub><b>Mohsen Esmailpour</b></sub></a><br /><a href="https://github.com/serilog-contrib/serilog-ui/commits?author=mo-esmp" title="Code">💻</a> <a href="#projectManagement-mo-esmp" title="Project Management">📆</a> <a href="https://github.com/serilog-contrib/serilog-ui/commits?author=mo-esmp" title="Tests">⚠️</a></td>
      <td align="center" valign="top" width="10%"><a href="https://www.matteogregoricchio.com/"><img src="https://avatars.githubusercontent.com/u/32459930?v=4?s=60" width="60px;" alt="Matteo Gregoricchio"/><br /><sub><b>Matteo Gregoricchio</b></sub></a><br /><a href="https://github.com/serilog-contrib/serilog-ui/commits?author=followynne" title="Code">💻</a> <a href="https://github.com/serilog-contrib/serilog-ui/commits?author=followynne" title="Documentation">📖</a> <a href="https://github.com/serilog-contrib/serilog-ui/commits?author=followynne" title="Tests">⚠️</a></td>
      <td align="center" valign="top" width="10%"><a href="https://github.com/sommmen"><img src="https://avatars.githubusercontent.com/u/8641495?v=4?s=60" width="60px;" alt="sommmen"/><br /><sub><b>sommmen</b></sub></a><br /><a href="https://github.com/serilog-contrib/serilog-ui/commits?author=sommmen" title="Code">💻</a></td>
      <td align="center" valign="top" width="10%"><a href="http://igecelabs.net/"><img src="https://avatars.githubusercontent.com/u/2063980?v=4?s=60" width="60px;" alt="Israel Gómez de Celis"/><br /><sub><b>Israel Gómez de Celis</b></sub></a><br /><a href="https://github.com/serilog-contrib/serilog-ui/commits?author=igece" title="Code">💻</a></td>
      <td align="center" valign="top" width="10%"><a href="https://github.com/traien"><img src="https://avatars.githubusercontent.com/u/16708328?v=4?s=60" width="60px;" alt="Osama Bashir"/><br /><sub><b>Osama Bashir</b></sub></a><br /><a href="https://github.com/serilog-contrib/serilog-ui/commits?author=traien" title="Code">💻</a></td>
      <td align="center" valign="top" width="10%"><a href="https://rmauro.dev/"><img src="https://avatars.githubusercontent.com/u/3687018?v=4?s=60" width="60px;" alt="Ricardo"/><br /><sub><b>Ricardo</b></sub></a><br /><a href="https://github.com/serilog-contrib/serilog-ui/commits?author=ricardodemauro" title="Code">💻</a></td>
      <td align="center" valign="top" width="10%"><a href="https://github.com/hansoncaleb"><img src="https://avatars.githubusercontent.com/u/8008108?v=4?s=60" width="60px;" alt="Caleb Hanson"/><br /><sub><b>Caleb Hanson</b></sub></a><br /><a href="https://github.com/serilog-contrib/serilog-ui/commits?author=hansoncaleb" title="Code">💻</a></td>
      <td align="center" valign="top" width="10%"><a href="https://github.com/Millarex"><img src="https://avatars.githubusercontent.com/u/55946676?v=4?s=60" width="60px;" alt="Aleksei"/><br /><sub><b>Aleksei</b></sub></a><br /><a href="https://github.com/serilog-contrib/serilog-ui/commits?author=Millarex" title="Code">💻</a></td>
      <td align="center" valign="top" width="10%"><a href="https://github.com/chaadfh"><img src="https://avatars.githubusercontent.com/u/133214342?v=4?s=60" width="60px;" alt="chaadfh"/><br /><sub><b>chaadfh</b></sub></a><br /><a href="https://github.com/serilog-contrib/serilog-ui/commits?author=chaadfh" title="Code">💻</a></td>
      <td align="center" valign="top" width="10%"><a href="https://github.com/phillduffy"><img src="https://avatars.githubusercontent.com/u/710550?v=4?s=60" width="60px;" alt="Phill Duffy"/><br /><sub><b>Phill Duffy</b></sub></a><br /><a href="https://github.com/serilog-contrib/serilog-ui/commits?author=phillduffy" title="Code">💻</a></td>
    </tr>
    <tr>
      <td align="center" valign="top" width="10%"><a href="https://github.com/uthmanrahimi"><img src="https://avatars.githubusercontent.com/u/45357615?v=4?s=60" width="60px;" alt="Uthman"/><br /><sub><b>Uthman</b></sub></a><br /><a href="https://github.com/serilog-contrib/serilog-ui/commits?author=uthmanrahimi" title="Code">💻</a></td>
      <td align="center" valign="top" width="10%"><a href="https://github.com/jorgevp"><img src="https://avatars.githubusercontent.com/u/3268148?v=4?s=60" width="60px;" alt="jorgevp"/><br /><sub><b>jorgevp</b></sub></a><br /><a href="https://github.com/serilog-contrib/serilog-ui/commits?author=jorgevp" title="Code">💻</a></td>
    </tr>
  </tbody>
</table>

<!-- markdownlint-restore -->
<!-- prettier-ignore-end -->

<!-- ALL-CONTRIBUTORS-LIST:END -->

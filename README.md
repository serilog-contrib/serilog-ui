# serilog-ui
A simple Serilog log viewer for following sinks:
- [Serilog.Sinks.MSSqlServer](https://github.com/serilog/serilog-sinks-mssqlserver)
- [Serilog.Sinks.MySql](https://github.com/TeleSoftas/serilog-sinks-mariadb)
- [Serilog.Sinks.Postgresql](https://github.com/b00ted/serilog-sinks-postgresql)
- [Serilog.Sinks.MongoDB](https://github.com/serilog/serilog-sinks-mongodb)
- [Serilog.Sinks.ElasticSearch](https://github.com/serilog/serilog-sinks-elasticsearch)

![serilog ui](https://raw.githubusercontent.com/mo-esmp/serilog-ui/master/assets/serilog-ui.jpg)

Install the _Serilog.UI_ [NuGet package](https://www.nuget.org/packages/Serilog.UI)
```powershell
Install-Package Serilog.UI
```

Then install one of the providers based upon your sink:

| Provider Name                  | Install                                          | Package                                                                        |
| ------------------------------ | ------------------------------------------------ | ------------------------------------------------------------------------------ |
| Serilog.UI.MsSqlServerProvider | `Install-Package Serilog.UI.MsSqlServerProvider` | [NuGet package](https://www.nuget.org/packages/Serilog.UI.MsSqlServerProvider) |
| Serilog.UI.MySqlProvider       | `Install-Package Serilog.UI.MySqlProvider`       | [NuGet package](https://www.nuget.org/packages/Serilog.UI.MySqlProvider)       |
| Serilog.UI.PostgreSqlProvider  | `Install-Package Serilog.UI.PostgreSqlProvider`  | [NuGet package](https://www.nuget.org/packages/Serilog.UI.PostgreSqlProvider)  |
| Serilog.UI.MongoDbProviderr    | `Install-Package Serilog.UI.MongoDbProvider`     | [NuGet package](https://www.nuget.org/packages/Serilog.UI.MongoDbProvider)     |
| Serilog.UI.ElasticSearchProvider | `Install-Package Serilog.UI.ElasticSearcProvider` | [NuGet package](https://www.nuget.org/packages/Serilog.UI.ElasticSearchProvider) |

Then, add `AddSerilogUi()` to `IServiceCollection` in `Startup.ConfigureServices` method:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Register the serilog UI services
    services.AddSerilogUi(options => options.UseSqlServer("ConnectionString", "LogTableName"));
}
```

In the `Startup.Configure` method, enable the middleware for serving logs UI. Place a call to the `UseSerilogUi` middleware after authentication and authorization middlewares otherwise authentication may not work for you:

```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    .
    .
    .

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
        
    // Enable middleware to serve log-ui (HTML, JS, CSS, etc.).
    app.UseSerilogUi();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
    });
}
```

Default url to view log page is `http://<your-app>/serilog-ui`. If you want to change this url path, just config route prefix:
```csharp
app.UseSerilogUi(option => option.RoutePrefix = "logs");
```
**Authorization configuration required**

By default serilog-ui allows access to log page only for local requests. In order to give appropriate rights for production use, you need to configuring authorization. You can secure log page by allwoing specific users or roles to view logs:
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddSerilogUi(options => options
        .EnableAuthorization(authOptions =>
        {
            authOption.AuthenticationType = AuthenticationType.Jwt; // or AuthenticationType.Cookie
            authOptions.Usernames = new[] { "User1", "User2" };
            authOptions.Roles = new[] { "AdminRole" };
        })
        .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), "LogTableName"));
    .
    .
    .
```
Only `User1` and `User2` or users with `AdminRole` role can view logs. If you set `AuthenticationType` to `Jwt`, you can set jwt token and `Authorization` header will be added to the request and for `Cookie` just login into you website and no extra step is required.

## Limitation
* Additional columns are not supported and only main columns can be retrieved

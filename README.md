# serilog-ui
A simple log viewer to see logs saved by [Serilog.Sinks.MSSqlServer](https://github.com/serilog/serilog-sinks-mssqlserver) (other sinks will be added in the future).

![serilog ui](https://raw.githubusercontent.com/mo-esmp/serilog-ui/master/assets/serilog-ui.jpg)

Install the _Serilog.UI_ [NuGet package](https://www.nuget.org/packages/Serilog.UI) and _Serilog.Ui.MsSqlServerProvider_ [NuGet package](https://www.nuget.org/packages/Serilog.Ui.MsSqlServerProvider)

```powershell
Install-Package Serilog.UI
Install-Package Serilog.UI.MsSqlServerProvider
```
or
```shell
dotnet add package Serilog.UI
dotnet add package Serilog.UI.MsSqlServerProvider
```

Then, add `UseSerilogUi()` to `IServiceCollection` in `ConfigureServices` method:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    var mvcBuilder = services.AddControllersWithViews();
    services.AddSerilogUi(mvcBuilder, options => options.UseSqlServer("ConnectionString", "LogTableName"));
    .
    .
    .
```

You can also secure log viewer by allwoing specific users or roles to view logs:
```csharp
public void ConfigureServices(IServiceCollection services)
{
    var mvcBuilder = services.AddControllersWithViews();
    services.AddSerilogUi(mvcBuilder, options => options
        .EnableAuthorization(authOptions =>
        {
            authOptions.Usernames = new[] { "User1", "User2" };
            authOptions.Roles = new[] { "AdminRole" };
        })
        .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), "LogTableName"));
    .
    .
    .
```
Only `User1` and `User2` or users with `AdminRole` role can view logs.

## Limitation
* Log url `/logs` is fix and cannot be changed
* Log viewer only works with MVC so you have to register views `services.AddControllersWithViews();` and also add default route `endpoints.MapControllerRoute( name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");`
* Additional columns are not supported and only main columns can be retrieved

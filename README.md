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

| Provider Name                    | Install                                           | Package                                                                          |
| -------------------------------- | ------------------------------------------------- | -------------------------------------------------------------------------------- |
| Serilog.UI.MsSqlServerProvider   | `Install-Package Serilog.UI.MsSqlServerProvider`  | [NuGet package](https://www.nuget.org/packages/Serilog.UI.MsSqlServerProvider)   |
| Serilog.UI.MySqlProvider         | `Install-Package Serilog.UI.MySqlProvider`        | [NuGet package](https://www.nuget.org/packages/Serilog.UI.MySqlProvider)         |
| Serilog.UI.PostgreSqlProvider    | `Install-Package Serilog.UI.PostgreSqlProvider`   | [NuGet package](https://www.nuget.org/packages/Serilog.UI.PostgreSqlProvider)    |
| Serilog.UI.MongoDbProviderr      | `Install-Package Serilog.UI.MongoDbProvider`      | [NuGet package](https://www.nuget.org/packages/Serilog.UI.MongoDbProvider)       |
| Serilog.UI.ElasticSearchProvider | `Install-Package Serilog.UI.ElasticSearcProvider` | [NuGet package](https://www.nuget.org/packages/Serilog.UI.ElasticSearchProvider) |

Then, add `AddSerilogUi()` to `IServiceCollection` in `Startup.ConfigureServices` method:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Register the serilog UI services
    services.AddSerilogUi(options => options.UseSqlServer("ConnectionString", "LogTableName"));
}
```

In the `Startup.Configure` method, enable the middleware for serving the log UI. Place a call to the `UseSerilogUi` middleware after authentication and authorization middlewares, otherwise authentication may not work for you:

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

## Authorization: configuration

By default serilog-ui allows access to the log page only for local requests. In order to give appropriate rights for production use, you need to configure authorization. You can secure the log page by allowing specific users or roles to view logs:

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
    // ...
}
```
Only `User1` and `User2` or users with `AdminRole` role can view logs.

If you set `AuthenticationType` to `Jwt`, you can set a jwt token and an `Authorization` header will be added to the request and for `Cookie` just login into you website and no extra step is required.

To disable anonymous access for local requests, (e.g. for testing authentication locally) set `AlwaysAllowLocalRequests` to `false`.

To disable authorization on production, set `Enabled` to false.

``` csharp
services.AddSerilogUi(options => options
    .EnableAuthorization(authOption =>
    {
        authOption.AlwaysAllowLocalRequests = false; // disable anonymous access on local
        authOption.Enabled = false; // disable authorization access check on production
    })
    .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), "Logs"));
```

## Options
Options can be found in the [UIOptions](src/Serilog.Ui.Web/Extensions/UiOptions.cs) class.
`internal` properties can generally be set via extension methods, see [SerilogUiOptionBuilderExtensions](src/Serilog.Ui.Web/Extensions/SerilogUiOptionBuilderExtensions.cs)

### Log page URL

The default url to view the log page is `http://<your-app>/serilog-ui`. If you want to change this url path, just configure the route prefix:

```csharp
app.UseSerilogUi(option => option.RoutePrefix = "logs");
```

### Home url
![image](https://user-images.githubusercontent.com/8641495/185874822-1d4b6f52-864c-4ffb-9064-6fc5ee9a079c.png)

The home button url can be customized by setting the `HomeUrl` property.

``` csharp
app.UseSerilogUi(options =>
{
    options.HomeUrl = "https://example.com/example?q=example";
});
```

### Inject custom Javascript and CSS

For customization of the dashboard UI, custom JS and CSS can be injected. 

CSS gets injected in the `<head>` element.

JS gets injected at the end of the `<body>` element by default. 

To inject JS in the `<head>` element set `injectInHead` to `true`.

``` csharp
app.UseSerilogUi(x =>
{
    x.InjectJavascript(path: "/js/serilog-ui/custom.js", injectInHead: false, type: "text/javascript");
    x.InjectStylesheet(path: "/css/serilog-ui/custom.css", media: "screen");
});
```

Custom JS/CSS files must be served by the backend via [static file middleware](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/static-files).

``` csharp
var builder = WebApplication.CreateBuilder(args);
...
app.UseStaticFiles();
...
```

With the default configuration static files are served under the wwwroot folder, so in the example above the file structure should be:

![image](https://user-images.githubusercontent.com/8641495/185877921-99aaf19a-3e62-4ad9-85c3-47994e7e6ba1.png)

JS code can be ran when loading the file by wrapping the code in a function, and directly running that function like so:
``` js
(function () {
    console.log("custom.js is loaded.");
})();
```

## UI: Frontend development

The serilog-ui frontend is located inside the Serilog.Ui.Web package.

Under the assets/ folder you can find all the relative files. 

The serilog-ui frontend is written in Typescript and built through [ParcelJS](https://parceljs.org/).  
***Requirement***: Node.js > 16  
Please run ```npm i``` to install all dependencies!

There are two Grunt tasks you can use to build the frontend project:

- **build**: it cleans wwwroot/dist/ and creates a production-ready build.
  This build can be used to either:
  - test the frontend development with the SampleWebApp
  - create a new production build, by committing the entire wwwroot/dist folder changes  
- **dev**: it cleans wwwroot/dev/ and starts a dev server to https://127.0.0.1/serilog-ui/  
  please notice that the development build uses [msw](https://mswjs.io/) to swap any serilog-ui fetch with a mocked version.  
  You can check details by reading ```assets/script/mocks/fetchMock.ts```.  

<details>
  <summary>Expand to read additional instructions</summary>
  
  Open solution with Visual Studio, to enable easier integration  
  right click on src/Serilog.Ui.Web/Grunfile.js => open "Task Runner Explorer"

  In the Task Explorer, double click on the Alias Tasks **dev**  
  You'll see the task starting Parcel task and staying in watch mode, with an output similar to:

  ```
  Server running at https://127.0.0.1:1234
  Building...
  Bundling...
  Packaging & Optimizing...
  √ Built in 2.84s
  ```

  When developing the assets (without the Serilog Middleware), no VS start is needed; Parcel starts a dev server with all you need for the assets part.  
  All fetches are mocked with MSW to serve fake data (saved in ***mocks/samples.ts***); this helps an user develop the assets without having to worry about creating actual data.

  You can open either https://localhost:1234 or https://127.0.0.1:1234 to work.
</details>

**please notice that you'll need to accept Parcel https self-signed certificate, otherwise msw service-worker won't be able to connect.**  
[Parcel related issue](https://github.com/parcel-bundler/parcel/issues/1746), [How to do it - example](https://www.pico.net/kb/how-do-you-get-chrome-to-accept-a-self-signed-certificate/

<details>
  <summary>If you see an https error (untrusted certificate) while developing with Parcel</summary>

  for Chrome:
  - click on Not Secure (next to the URL) => click Certificate is not valid => click Details => Copy To File => export the cert as DER Encoded Binary X.509 .cer
  - go to: chrome://settings/security => click Manage Certificates => go to Trusted Root Certification Authorities tab => import the .cer file previously exported
  - restart Chrome
  - you should be able to run the dev environment on both localhost and 127.0.0.1 (to check if it's working fine, open the console: you'll find a red message: **"[MSW] Mocking enabled."**)
</details>

# Known Limitations
* Additional columns are not supported and only main columns can be retrieved.

# Test

## .NET Serilog.UI Projects

The test projects are located inside the */tests* folder.

Each Serilog.Ui project has a separate test project.
Each project is based onto the **xUnit** test framework.

**Serilog.Ui.Common.Tests**: contains anything that can be shared between the tests projects.

To run the tests, use Test Explorer in Visual Studio or run from the root folder:

```
dotnet test
```

## JS UI assets

Tests are located inside src/Serilog.Ui.Web/assets/__tests__

Tests are based onto Jest test framework, with the help of JSDOM and testing-library packages. Any HTTP request is mocked through [msw](https://mswjs.io/).

Jest configuration can be found in src/Serilog.Ui.Web/jest.config.js; any additional setup item is located inside src/Serilog.Ui.Web/assets/__tests__/util (this folder is excluded from test runs).

To run the tests, open a terminal in src/Serilog.Ui.Web/ and launch this command (watch-mode):

```
npm test
```
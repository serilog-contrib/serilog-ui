using System.Data;
using System.Text.Json;
using System.Xml.Linq;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using Serilog.Ui.Core.OptionsBuilder;
using Serilog.Ui.MsSqlServerProvider.Extensions;
using Serilog.Ui.Web.Extensions;
using WebApi.HostedServices;
using WebApi.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WebApi.Extensions;

internal static class ServiceCollectionExtensions
{
    private static ColumnOptions Opts()
    {
        var cols = new ColumnOptions
        {
            AdditionalColumns = new List<SqlColumn>
            {
                new(nameof(TestLogModel.SampleDate), SqlDbType.DateTime),
                new(nameof(TestLogModel.SampleDateOffset), SqlDbType.DateTimeOffset),
                new(nameof(TestLogModel.SampleBool), SqlDbType.Bit),
                new(nameof(TestLogModel.EnvironmentName), SqlDbType.VarChar),
                new(nameof(TestLogModel.EnvironmentUserName), SqlDbType.VarChar),
                new(nameof(TestLogModel.SampleJsonCodeColumn), SqlDbType.VarChar),
                new(nameof(TestLogModel.SampleXmlCodeColumn), SqlDbType.VarChar),
            }
        };
        cols.Store.Remove(StandardColumn.Properties);
        return cols;
    }

    private static readonly string JsonOutput = JsonSerializer.Serialize(new { test = "my-prop", other = 2 });

    private static string XmlOutput
    {
        get
        {
            var jsonDocument = JsonDocument.Parse(JsonOutput);
            var xDocument = new XDocument(
                new XElement("Root",
                    jsonDocument.RootElement.EnumerateObject()
                        .Select(prop => new XElement(prop.Name, prop.Value.ToString()))
                )
            );
            return xDocument.ToString(SaveOptions.DisableFormatting);
        }
    }

    internal static void ConfigureSerilog(this ConfigureHostBuilder configureHostBuilder)
    {
        configureHostBuilder.UseSerilog((_, loggerConfiguration) =>
        {
            loggerConfiguration
                .Enrich.WithEnvironmentName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithProperty(nameof(TestLogModel.SampleJsonCodeColumn), JsonOutput)
                .Enrich.WithProperty(nameof(TestLogModel.SampleXmlCodeColumn), XmlOutput)
                .Enrich.AtLevel(LogEventLevel.Information, p =>
                {
                    p.WithProperty(nameof(TestLogModel.SampleBool), true);
                    p.WithProperty(nameof(TestLogModel.SampleDate), new DateTime(2022, 01, 15, 10, 00, 00));
                    p.WithProperty(nameof(TestLogModel.SampleDateOffset),
                        new DateTimeOffset(2022, 01, 15, 10, 00, 00, TimeSpan.FromHours(-2)));
                })
                .Enrich.FromLogContext()
                .MinimumLevel.Verbose()
                // configuration required to start ms sql logging only AFTER test container is running
                // ref: https://nblumhardt.com/2023/02/dynamically-reload-any-serilog-sink/
                .WriteTo.Map(
                    _ => SqlServerContainerService.SqlConnectionString,
                    (url, wt) =>
                    {
                        if (!string.IsNullOrWhiteSpace(url))
                        {
                            wt.MSSqlServer(
                                    url,
                                    new MSSqlServerSinkOptions { TableName = "logs", AutoCreateSqlTable = true },
                                    columnOptions: Opts())
                                .WriteTo.MSSqlServer(url, new MSSqlServerSinkOptions { TableName = "logsBackup", AutoCreateSqlTable = true });
                        }
                    });
        });
    }

    /// <summary>
    /// Check this method to find some sample configuration for Serilog UI.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    internal static IServiceCollection AddSerilogUiSample(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddSerilogUi(options =>
                {
                    options
                        /* samples: authorization filters */
                        // a] default filter, sync, authorize only local requests 
                        .AddScopedAuthorizeLocalRequestsAuthFilter()
                        /* sample provider registration: Sql Server [multiple], Fluent interface */
                        .UseSqlServer<TestLogModel>(opt =>
                            opt.WithConnectionString(configuration.GetConnectionString("MsSqlDefaultConnection")).WithTable("logs"))
                        .UseSqlServer(opt =>
                            opt.WithConnectionString(configuration.GetConnectionString("MsSqlBackupConnection")).WithTable("logsBackup"));
                }
            );
}
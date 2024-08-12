using System.Data;
using System.Text.Json;
using System.Xml.Linq;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.MSSqlServer;
using Serilog.Ui.Core.Extensions;
using Serilog.Ui.Core.Interfaces;
using Serilog.Ui.ElasticSearchProvider.Extensions;
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

    internal static void ConfigureSerilog(this ConfigureHostBuilder configureHostBuilder, bool enableElasticSample)
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
                .WriteToElastic(enableElasticSample)
                // configuration required to start ms sql logging only AFTER test containers are running
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
    /// Unfortunately TestContainers does not support NEST package and requires Elastic.Clients.Elasticsearch (NEST v8) <br />
    /// To start an Elasticsearch container that works with sink and provider, run:
    /// docker run -d --name elasticsearch --net elastic -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" -e "xpack.security.enabled=false" elasticsearch:8.13.0
    /// </summary>
    private static LoggerConfiguration WriteToElastic(this LoggerConfiguration logger, bool enableElasticSample)
    {
        if (!enableElasticSample) return logger;

        return logger.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
        {
            AutoRegisterTemplate = true,
            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
            IndexFormat = "logs-7x-default-{0:yyyy.MM.dd}",
            TemplateName = "serilog-logs-7x",
            BatchAction = ElasticOpType.Create,
            ModifyConnectionSettings = c => c.EnableApiVersioningHeader()
        });
    }

    /// <summary>
    /// Check this method to find some sample configuration for Serilog UI.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="enableElasticSample"></param>
    /// <returns></returns>
    internal static IServiceCollection AddSerilogUiSample(this IServiceCollection services, IConfiguration configuration, bool enableElasticSample)
        => services
            .AddSerilogUi(options =>
                {
                    options
                        /* samples: authorization filters */
                        // a] default filter, sync, authorize only local requests 
                        .AddScopedAuthorizeLocalRequestsAuthFilter()
                        /* sample provider registration: Sql Server [multiple], Fluent interface */
                        .UseSqlServer<TestLogModel>(opt => opt
                            .WithConnectionString(configuration.GetConnectionString("MsSqlDefaultConnection")!)
                            .WithTable("logs"))
                        .UseSqlServer(opt => opt
                            .WithConnectionString(configuration.GetConnectionString("MsSqlBackupConnection")!)
                            .WithTable("logsBackup"))
                        .UseElasticSample(enableElasticSample);
                }
            );

    private static void UseElasticSample(this ISerilogUiOptionsBuilder builder, bool enableElasticSample)
    {
        if (!enableElasticSample) return;

        /* sample provider registration: ElasticSearch, Fluent interface */
        builder.UseElasticSearchDb(opt =>
            opt.WithEndpoint(new Uri("http://localhost:9200/")).WithIndex($"logs-7x-default-{DateTime.UtcNow:yyyy.MM.dd}"));
    }
}
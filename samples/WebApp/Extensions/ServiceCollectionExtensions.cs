using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Ui.Core.Extensions;
using Serilog.Ui.Core.OptionsBuilder;
using Serilog.Ui.MongoDbProvider.Extensions;
using Serilog.Ui.Web.Authorization.Filters;
using Serilog.Ui.Web.Extensions;
using WebApp.Authentication;
using WebApp.Authentication.Jwt;

namespace WebApp.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddAuthenticationDetails(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthorizationBuilder()
            .AddPolicy("test-policy", (builder) =>
                {
                    // a sample policy, checking for a custom claim
                    builder.RequireClaim("example", "my-value");
                });

        services
            .AddScoped<JwtTokenGenerator>()
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = JwtKeyGenerator.Generate(configuration["Jwt:SecretKey"])
                    };
            });

        return services;
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
            // custom filter implementation, sync
            .AddScopedSyncAuthFilter<SerilogUiCustomAuthFilter>()
            // default filter, async, checks a configured authorization policy
            .AddScopedPolicyAuthFilter("test-policy")
            /* sample provider registration: MongoDb, Fluent interface */
            .UseMongoDb(opt => opt
                .WithConnectionString(configuration.GetConnectionString("MongoDbDefaultConnection"))
                .WithCollectionName("logs"));

            /* sample provider registration: Sql Server [multiple], Fluent interface
             * .UseSqlServer(opt => opt.WithConnectionString(builder.Configuration.GetConnectionString("MsSqlDefaultConnection")).WithTable("Logs"))
             * .UseSqlServer(opt => opt.WithConnectionString(builder.Configuration.GetConnectionString("MsSqlBackupConnection")).WithTable("LogsBackup"))
             */
        }
);
}
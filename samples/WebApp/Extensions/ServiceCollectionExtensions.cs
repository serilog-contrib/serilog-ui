using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Serilog.Ui.Core.Extensions;
using Serilog.Ui.MongoDbProvider.Extensions;
using Serilog.Ui.Web.Extensions;
using WebApp.Authentication;
using WebApp.Authentication.Jwt;

namespace WebApp.Extensions;

internal static class ServiceCollectionExtensions
{
    private static void PoliciesConfig(AuthorizationPolicyBuilder builder)
    {
        // a sample policy, checking for a custom claim
        builder.RequireClaim("example", "my-value");
    }

    internal static IServiceCollection AddAuthenticationDetails(this IServiceCollection services, IConfiguration configuration)
    {
        services
#if (NET7_0_OR_GREATER)
            .AddAuthorizationBuilder()
            .AddPolicy("test-policy", PoliciesConfig);
#else
            .AddAuthorization(builder => builder.AddPolicy("test-policy", PoliciesConfig));
#endif

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
                        // a] custom filter implementation, sync, checks that user is Authenticated
                        .AddScopedSyncAuthFilter<SerilogUiCustomAuthFilter>()
                        // b] default filter, async, checks against configured authorization policy
                        .AddScopedPolicyAuthFilter("test-policy")
                        // c] default filter, async, checks a basic header against a predefined IConfiguration user-password
                        // .AddScopedBasicAuthFilter()
                        /* sample provider registration: MongoDb, Fluent interface */
                        .UseMongoDb(opt => opt
                            .WithConnectionString(configuration.GetConnectionString("MongoDbDefaultConnection")!)
                            .WithCustomProviderName("MyMongoDbCollection")
                            .WithCollectionName("logs")
                        );
                }
            );
}
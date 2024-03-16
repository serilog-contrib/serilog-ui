using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Interfaces;
using Serilog.Ui.Web.Authorization.Filters;

namespace Serilog.Ui.Web.Extensions;

/// <summary>
/// Extension methods for <see cref="SerilogUiOptionsBuilder"/>.
/// </summary>
public static class SerilogUiOptionBuilderExtensions
{
    /// <summary>
    /// Add <see cref="PolicyAuthorizationFilter"/> as scoped service implementation of <see cref="IUiAsyncAuthorizationFilter"/>.
    /// </summary>
    public static ISerilogUiOptionsBuilder AddScopedPolicyAuthFilter(this ISerilogUiOptionsBuilder builder, string policy)
    {
        builder.Services
            .AddScoped<IUiAsyncAuthorizationFilter, PolicyAuthorizationFilter>(sp => new PolicyAuthorizationFilter(
                sp.GetRequiredService<IHttpContextAccessor>(),
                sp.GetRequiredService<IAuthorizationService>(),
                policy));

        return builder;
    }
}
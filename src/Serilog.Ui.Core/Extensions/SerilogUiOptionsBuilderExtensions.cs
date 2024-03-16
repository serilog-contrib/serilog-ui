using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core.Interfaces;

namespace Serilog.Ui.Core.Extensions;

/// <summary>
/// SerilogUiOptionsBuilderExtensions.
/// </summary>
public static class SerilogUiOptionsBuilderExtensions
{
    /// <summary>
    /// Add <see cref="T"/> as scoped service implementation of <see cref="IUiAsyncAuthorizationFilter"/>.
    /// </summary>
    public static ISerilogUiOptionsBuilder AddScopedAsyncAuthFilter<T>(this ISerilogUiOptionsBuilder builder)
        where T : class, IUiAsyncAuthorizationFilter
    {
        builder.Services.AddScoped<IUiAsyncAuthorizationFilter, T>();

        return builder;
    }
    
    /// <summary>
    /// Add <see cref="T"/> as scoped service implementation of <see cref="IUiAuthorizationFilter"/>.
    /// </summary>
    public static ISerilogUiOptionsBuilder AddScopedSyncAuthFilter<T>(this ISerilogUiOptionsBuilder builder)
        where T : class, IUiAuthorizationFilter
    {
        builder.Services.AddScoped<IUiAuthorizationFilter, T>();

        return builder;
    }
}
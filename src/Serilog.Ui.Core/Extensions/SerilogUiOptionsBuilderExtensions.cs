using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core.Interfaces;

namespace Serilog.Ui.Core.Extensions;

/// <summary>
/// SerilogUiOptionsBuilderExtensions.
/// </summary>
public static class SerilogUiOptionsBuilderExtensions
{
    /// <summary>
    /// Add <typeparamref name="T"/> as scoped service implementation of <see cref="IUiAsyncAuthorizationFilter"/>,
    /// using an implementation factory.
    /// </summary>
    public static ISerilogUiOptionsBuilder AddScopedAsyncAuthFilter<T>(this ISerilogUiOptionsBuilder builder,
        Func<IServiceProvider, T> implementationFactory) where T : class, IUiAsyncAuthorizationFilter
    {
        builder.Services.AddScoped<IUiAsyncAuthorizationFilter, T>(implementationFactory);

        return builder;
    }

    /// <summary>
    /// Add <typeparamref name="T"/> as scoped service implementation of <see cref="IUiAsyncAuthorizationFilter"/>.
    /// </summary>
    public static ISerilogUiOptionsBuilder AddScopedAsyncAuthFilter<T>(this ISerilogUiOptionsBuilder builder)
        where T : class, IUiAsyncAuthorizationFilter
    {
        builder.Services.AddScoped<IUiAsyncAuthorizationFilter, T>();

        return builder;
    }

    /// <summary>
    /// Add <typeparamref name="T"/> as scoped service implementation of <see cref="IUiAuthorizationFilter"/>,
    /// using an implementation factory.
    /// </summary>
    public static ISerilogUiOptionsBuilder AddScopedSyncAuthFilter<T>(this ISerilogUiOptionsBuilder builder,
        Func<IServiceProvider, T> implementationFactory) where T : class, IUiAuthorizationFilter
    {
        builder.Services.AddScoped<IUiAuthorizationFilter, T>(implementationFactory);

        return builder;
    }

    /// <summary>
    /// Add <typeparamref name="T"/> as scoped service implementation of <see cref="IUiAuthorizationFilter"/>.
    /// </summary>
    public static ISerilogUiOptionsBuilder AddScopedSyncAuthFilter<T>(this ISerilogUiOptionsBuilder builder)
        where T : class, IUiAuthorizationFilter
    {
        builder.Services.AddScoped<IUiAuthorizationFilter, T>();

        return builder;
    }
}
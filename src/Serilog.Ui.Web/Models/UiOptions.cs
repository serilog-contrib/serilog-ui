﻿using System.Collections.ObjectModel;
using System.Text;
using Serilog.Ui.Core.Models;
using Serilog.Ui.Core.Models.Options;

namespace Serilog.Ui.Web.Models;

/// <summary>
/// Options used by SerilogUI to configure the log dashboard.
/// </summary>
public class UiOptions(ProvidersOptions options)
{
    /// <summary>
    /// Gets the serilog-ui brand visibility, defaults to true.
    /// </summary>
    public bool ShowBrand { get; private set; } = true;

    /// <summary>
    /// Gets the URL for the home button.
    /// </summary>
    /// <value>The URL for the home button.</value>
    public string HomeUrl { get; private set; } = "/";

    /// <summary>
    /// Gets the route prefix to access log dashboard via browser.
    /// </summary>
    /// <value>The route prefix.</value>
    public string RoutePrefix { get; private set; } = "serilog-ui";

    /// <summary>
    /// Gets the server sub-path deployment.
    /// </summary>
    /// <value>The server sub path.</value>
    public string ServerSubPath { get; private set; } = string.Empty;

    /// <summary>
    /// Get the option to auto-expand dropdowns in the log viewer.
    /// </summary>
    public bool ExpandDropdownsByDefault { get; private set; }

    /// <summary>
    /// Sets the type of the authentication.
    /// </summary>
    public UiOptions WithAuthenticationType(AuthenticationType authType)
    {
        Authorization.AuthenticationType = authType;
        return this;
    }

    /// <summary>
    /// Run the authorization filters on the app routes.
    /// </summary>
    public UiOptions EnableAuthorizationOnAppRoutes()
    {
        Authorization.RunAuthorizationFilterOnAppRoutes = true;
        return this;
    }

    /// <summary>
    /// Sets the serilog-ui brand visibility to false.
    /// </summary>
    public UiOptions HideSerilogUiBrand()
    {
        ShowBrand = false;
        return this;
    }

    /// <summary>
    /// Sets the expand dropdowns by default property to true.
    /// </summary>
    public UiOptions WithExpandedDropdownsByDefault()
    {
        ExpandDropdownsByDefault = true;
        return this;
    }

    /// <summary>
    /// Injects additional CSS stylesheets into the index.html page.
    /// Each call to the method adds a stylesheet entry.
    /// </summary>
    /// <param name="path">A path to the stylesheet - i.e. the link "href" attribute</param>
    /// <param name="media">The target media - i.e. the link "media" attribute</param>
    /// <returns>The passed options object for chaining</returns>
    public UiOptions InjectStylesheet(string path, string media = "screen")
    {
        var builder = new StringBuilder(HeadContent);
        builder.AppendLine($"<link href='{path}' rel='stylesheet' media='{media}' type='text/css' />");
        HeadContent = builder.ToString();

        return this;
    }

    /// <summary>
    /// Injects additional Javascript files into the index.html page.
    /// Each call to the method adds a stylesheet entry.
    /// </summary>
    /// <param name="path">A path to the javascript - i.e. the script "src" attribute</param>
    /// <param name="injectInHead">
    ///   When true, injects the javascript in the &lt;head&gt; tag instead of the &lt;body&gt; tag
    /// </param>
    /// <param name="type">The script type - i.e. the script "type" attribute</param>
    /// <returns>The passed options object for chaining</returns>
    public UiOptions InjectJavascript(string path, bool injectInHead = false, string type = "text/javascript")
    {
        var builder = new StringBuilder(injectInHead ? HeadContent : BodyContent);
        builder.AppendLine($"<script src='{path}' type='{type}'></script>");
        if (injectInHead)
            HeadContent = builder.ToString();
        else
            BodyContent = builder.ToString();

        return this;
    }

    /// <summary>
    /// Sets the URL for the home button.
    /// </summary>
    public UiOptions WithHomeUrl(string homeUrl)
    {
        HomeUrl = homeUrl;
        return this;
    }

    /// <summary>
    /// Sets the route prefix to access log dashboard via browser.
    /// The default value is <c>serilog-ui</c> and you can the dashboard by using <c>{base-path}/serilog-ui/</c>.
    /// It MUST not end with a slash.
    /// </summary>
    public UiOptions WithRoutePrefix(string routePrefix)
    {
        RoutePrefix = routePrefix;
        return this;
    }

    /// <summary>
    /// Sets the server hosting base-path.
    /// If the application is deployed under a server sub-path, this property provides the additional sub-path to
    /// the dashboard to help the client-side routing.<br/>
    /// If provided, the final routing result will be:<br/><c>{server-base-path}/{UiOptions.ServerSubPath}/{UiOptions.RoutePrefix}</c>.
    /// </summary>
    /// <example>
    /// Server root: "server.com"
    /// IF the application is deployed at root level ("server.com/{my-app}") => NOT set ServerSubPath
    /// IF the application is deployed at sub-path level ("server.com/{additional-path}/{my-app}") => MUST set ServerSubPath
    /// </example>
    public UiOptions WithServerSubPath(string serverSubPath)
    {
        ServerSubPath = serverSubPath;
        return this;
    }

    internal void Validate()
    {
        if (RoutePrefix.EndsWith('/')) throw new ArgumentException($"{nameof(RoutePrefix)} can't end with a slash.");
    }

    #region internals

    /// <summary>
    /// Gets <see cref="AuthorizationOptions"/> info.
    /// </summary>
    internal AuthorizationOptions Authorization { get; private set; } = new();

    /// <summary>
    /// Gets or sets the head content, a string that will be placed in the &lt;body&gt; of the index.html
    /// </summary>
    /// <value>The head content.</value>
    internal string BodyContent { get; set; } = string.Empty;

    /// <summary>
    /// Gets ColumnsInfo.
    /// </summary>
    internal readonly ReadOnlyDictionary<string, ColumnsInfo> ColumnsInfo = options.AdditionalColumns;

    /// <summary>
    /// Gets or sets the database keys that can't change the sort property.
    /// </summary>
    internal IEnumerable<string> DisabledSortOnKeys { get; } = options.DisabledSortProviderNames;

    /// <summary>
    /// Gets or sets the database keys that renders exceptions as simple strings.
    /// </summary>
    internal IEnumerable<string> RenderExceptionAsStringKeys { get; } = options.ExceptionAsStringProviderNames;

    /// <summary>
    ///   Gets or sets the head content, a string that will be placed in the &lt;head&gt; of the index.html
    /// </summary>
    /// <value>The head content.</value>
    internal string HeadContent { get; set; } = string.Empty;

    #endregion internals
}
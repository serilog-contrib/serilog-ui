using System.Collections.Generic;
using System.Collections.ObjectModel;
using Serilog.Ui.Core.OptionsBuilder;

namespace Serilog.Ui.Web.Models;

/// <summary>
/// Options used by SerilogUI to configure the log dashboard.
/// </summary>
public class UiOptions(ProvidersOptions options)
{
    /// <summary>
    ///   Gets or sets the type of the Filters.
    /// </summary>
    /// <value>The type of the authentication.</value>
    public AuthorizationOptions Authorization { get; set; } = new();

    /// <summary>
    ///   Gets or sets the serilog-ui brand visibility.
    /// </summary>
    public bool HideBrand { get; set; }

    /// <summary>
    ///   Gets or sets the URL for the home button
    /// </summary>
    /// <value>The URL for the home button.</value>
    public string HomeUrl { get; set; } = "/";

    /// <summary>
    /// Gets or sets the route prefix to access log dashboard via browser. The default value
    /// is <c>serilog-ui</c> and you can the dashboard by using <c>{base-path}/serilog-ui/</c>
    /// </summary>
    /// <value>The route prefix.</value>
    public string RoutePrefix { get; set; } = "serilog-ui";
    
    #region internals

    /// <summary>
    ///   Gets or sets the head content, a string that will be placed in the &lt;body&gt; of the index.html
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
    ///   Gets or sets the head content, a string that will be placed in the &lt;head&gt; of the index.html
    /// </summary>
    /// <value>The head content.</value>
    internal string HeadContent { get; set; } = string.Empty;

    #endregion
}
namespace Serilog.Ui.Web.Models;

/// <summary>
/// The options to be used by SerilogUI to log access authorization.
/// </summary>
internal class AuthorizationOptions
{
    /// <summary>
    /// Gets the type of the authentication. Defaults to <see cref="Models.AuthenticationType.Custom"/>.
    /// </summary>
    /// <value>The type of the authentication.</value>
    public AuthenticationType AuthenticationType { get; set; } = AuthenticationType.Custom;

    /// <summary>
    /// Gets if the authorization filters should be run when accessing the serilog-ui main pages.
    /// Used to block unauthorized access to the ui.
    /// Defaults to false.
    /// </summary>
    public bool RunAuthorizationFilterOnAppRoutes { get; set; }
}
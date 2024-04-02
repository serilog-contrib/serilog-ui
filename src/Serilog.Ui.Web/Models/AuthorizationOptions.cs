namespace Serilog.Ui.Web.Models
{
    /// <summary>
    ///   The options to be used by SerilogUI to log access authorization.
    /// </summary>
    public class AuthorizationOptions
    {
        /// <summary>
        ///   Gets or sets the type of the authentication.
        /// </summary>
        /// <value>The type of the authentication.</value>
        public AuthenticationType AuthenticationType { get; set; } = AuthenticationType.Custom;

        /// <summary>
        /// Set to true if the authorization filters should be run
        /// when accessing the serilog-ui main pages.
        /// Used to block unauthorized access to the logs ui.
        /// </summary>
        public bool RunAuthorizationFilterOnAppRoutes { get; set; } = false;
    }

    public enum AuthenticationType
    {
        Custom,

        Basic,

        Jwt,
    }
}
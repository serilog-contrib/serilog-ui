using Serilog.Ui.Web.Authorization;
using System.Collections.Generic;

namespace Serilog.Ui.Web
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
        public AuthenticationType AuthenticationType { get; set; } = AuthenticationType.Cookie;

        /// <summary>
        ///   Gets or sets the authorized usernames.
        /// </summary>
        /// <value>The usernames.</value>
        public IEnumerable<IUiAuthorizationFilter> Filters { get; set; } = new List<IUiAuthorizationFilter>()
        {
            new LocalRequestsOnlyAuthorizationFilter()
        };
    }

    public enum AuthenticationType
    {
        Cookie,
        Jwt,
        Windows
    }
}
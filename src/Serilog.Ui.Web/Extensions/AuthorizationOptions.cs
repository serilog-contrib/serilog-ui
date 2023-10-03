using Serilog.Ui.Web.Authorization;
using System;
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
        ///   Gets or sets the Authorization filters.
        /// </summary>
        public IEnumerable<IUiAuthorizationFilter> Filters { get; set; } = new List<IUiAuthorizationFilter>()
        {
            new LocalRequestsOnlyAuthorizationFilter()
        };

        /// <summary>
        ///   Gets or sets the AsyncAuthorization filters.
        /// </summary>
        public IEnumerable<IUiAsyncAuthorizationFilter> AsyncFilters { get; set; } = Array.Empty<IUiAsyncAuthorizationFilter>();

        /// <summary>
        /// Set to true if the authorization filters should be run
        /// when accessing the serilog-ui main pages.
        /// Used to block unauthorized access to the logs ui.
        /// </summary>
        public bool RunAuthorizationFilterOnAppRoutes { get; set; } = false;
    }

    public enum AuthenticationType
    {
        Cookie,
        Jwt,
        Windows
    }
}
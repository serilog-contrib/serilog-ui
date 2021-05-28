using System.Collections.Generic;

namespace Serilog.Ui.Web
{
    /// <summary>
    ///     The options to be used by SerilogUI to log access authorization.
    /// </summary>
    public class AuthorizationOptions
    {
        /// <summary>
        ///     Gets or sets the type of the authentication.
        /// </summary>
        /// <value> The type of the authentication. </value>
        public AuthenticationType AuthenticationType { get; set; }

        /// <summary>
        ///     Gets or sets the authorized usernames.
        /// </summary>
        /// <value> The usernames. </value>
        public IEnumerable<string> Usernames { get; set; }

        /// <summary>
        ///     Gets or sets the authorized roles.
        /// </summary>
        /// <value> The roles. </value>
        public IEnumerable<string> Roles { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="AuthorizationOptions"/> is enabled.
        /// </summary>
        /// <value> <c> true </c> if enabled; otherwise, <c> false </c>. </value>
        internal bool Enabled { get; set; } = false;
    }

    public enum AuthenticationType
    {
        Cookie,
        Jwt,
        Windows
    }
}
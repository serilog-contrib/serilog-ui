using System.Collections.Generic;

namespace Serilog.Ui.Web
{
    public class AuthorizationOptions
    {
        public AuthenticationType AuthenticationType { get; set; }

        public IEnumerable<string> Usernames { get; set; }

        public IEnumerable<string> Roles { get; set; }

        internal bool Enabled { get; set; } = false;
    }

    public enum AuthenticationType
    {
        Cookie,
        Jwt,
        Windows
    }
}
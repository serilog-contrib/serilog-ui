using System.Collections.Generic;

namespace Serilog.Ui.Web
{
    public class AuthorizationOptions
    {
        public IEnumerable<string> Usernames { get; set; }

        public IEnumerable<string> Roles { get; set; }

        internal bool Enabled { get; set; } = false;
    }
}
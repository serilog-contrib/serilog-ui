namespace Serilog.Ui.Web
{
    public class UiOptions
    {
        public string RoutePrefix { get; set; } = "serilog-ui";

        internal AuthenticationType AuthType { get; set; } = AuthenticationType.Cookie;
    }
}
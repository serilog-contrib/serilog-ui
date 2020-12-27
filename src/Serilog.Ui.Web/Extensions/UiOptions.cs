namespace Serilog.Ui.Web
{
    public class UiOptions
    {
        public string RoutePrefix { get; set; } = "serilog-ui";

        internal string AuthType { get; set; }
    }
}
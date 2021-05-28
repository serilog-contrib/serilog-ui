namespace Serilog.Ui.Web
{
    /// <summary>
    ///     The options to be used by SerilogUI to configure log dashboard. You normally use a <see
    ///     cref="Core.SerilogUiOptionsBuilder"/> to create instances of this class.
    /// </summary>
    public class UiOptions
    {
        /// <summary>
        ///     Gets or sets the route prefix to access log dashboard via browser. The default value
        ///     is <c> serilog-ui </c> and you can the dashboard by using <c>
        ///     http://localhost/serilog-ui </c>
        /// </summary>
        /// <value> The route prefix. </value>
        public string RoutePrefix { get; set; } = "serilog-ui";

        /// <summary>
        ///     Gets or sets the type of the authentication.
        /// </summary>
        /// <value> The type of the authentication. </value>
        internal string AuthType { get; set; }
    }
}
using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using System;
using System.Text;

namespace Serilog.Ui.Web
{
    /// <summary>
    ///     Extension methods for <see cref="SerilogUiOptionsBuilder"/>.
    /// </summary>
    public static class SerilogUiOptionBuilderExtensions
    {
        /// <summary>
        ///     In order to give appropriate rights for production use, you need to enables
        ///     configuring authorization. By default only local requests have access to the log
        ///     dashboard page.
        /// </summary>
        /// <param name="optionsBuilder"> The builder being used to configure the SerilogUI. </param>
        /// <param name="options"> An action to allow configure authorization. </param>
        /// <returns> SerilogUiOptionsBuilder. </returns>
        /// <exception cref="ArgumentNullException"> Throw if optionsBuilder is null </exception>
        /// <exception cref="ArgumentNullException"> Throw if options is null </exception>
        public static SerilogUiOptionsBuilder EnableAuthorization(this SerilogUiOptionsBuilder optionsBuilder, Action<AuthorizationOptions> options)
        {
            if (optionsBuilder == null)
                throw new ArgumentNullException(nameof(optionsBuilder));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            var authorizationOptions = new AuthorizationOptions();
            options(authorizationOptions);

            ((ISerilogUiOptionsBuilder)optionsBuilder).Services.AddSingleton(authorizationOptions);

            return optionsBuilder;
        }

        /// <summary>
        /// Injects additional CSS stylesheets into the index.html page
        /// </summary>
        /// <param name="options"></param>
        /// <param name="path">A path to the stylesheet - i.e. the link "href" attribute</param>
        /// <param name="media">The target media - i.e. the link "media" attribute</param>
        /// <returns>The passed options object for chaining</returns>
        public static UiOptions InjectStylesheet(this UiOptions options, string path, string media = "screen")
        {
            var builder = new StringBuilder(options.HeadContent);
            builder.AppendLine($"<link href='{path}' rel='stylesheet' media='{media}' type='text/css' />");
            options.HeadContent = builder.ToString();
            return options;
        }

        /// <summary>
        /// Injects additional Javascript files into the index.html page
        /// </summary>
        /// <param name="options"></param>
        /// <param name="path">A path to the javascript - i.e. the script "src" attribute</param>
        /// <param name="injectInHead">When true, injects the javascript in the &lt;head&gt; tag instead of the &lt;body&gt; tag</param>
        /// <param name="type">The script type - i.e. the script "type" attribute</param>
        /// <returns>The passed options object for chaining</returns>
        public static UiOptions InjectJavascript(this UiOptions options, string path, bool injectInHead = false, string type = "text/javascript")
        {
            var builder = new StringBuilder(injectInHead ? options.HeadContent : options.BodyContent);
            builder.AppendLine($"<script src='{path}' type='{type}'></script>");
            if(injectInHead)
                options.HeadContent = builder.ToString();
            else
                options.BodyContent = builder.ToString();
            return options;
        }

    }
}
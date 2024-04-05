using System.Text;
using Serilog.Ui.Web.Models;

namespace Serilog.Ui.Web.Extensions;

public static class UiOptionsExtensions
{
    /// <summary>
    ///   Injects additional CSS stylesheets into the index.html page
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
    ///   Injects additional Javascript files into the index.html page
    /// </summary>
    /// <param name="options"></param>
    /// <param name="path">A path to the javascript - i.e. the script "src" attribute</param>
    /// <param name="injectInHead">
    ///   When true, injects the javascript in the &lt;head&gt; tag instead of the &lt;body&gt; tag
    /// </param>
    /// <param name="type">The script type - i.e. the script "type" attribute</param>
    /// <returns>The passed options object for chaining</returns>
    public static UiOptions InjectJavascript(this UiOptions options, string path, bool injectInHead = false, string type = "text/javascript")
    {
        var builder = new StringBuilder(injectInHead ? options.HeadContent : options.BodyContent);
        builder.AppendLine($"<script src='{path}' type='{type}'></script>");
        if (injectInHead)
            options.HeadContent = builder.ToString();
        else
            options.BodyContent = builder.ToString();
        return options;
    }
}
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.Ui.Web
{
    [HtmlTargetElement("truncate")]
    public class TruncateTagHelper : TagHelper
    {
        [HtmlAttributeName("length")]
        public int Length { get; set; }

        [HtmlAttributeName("truncate")]
        public ModelExpression Truncate { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (output == null)
                throw new ArgumentNullException(nameof(output));

            await base.ProcessAsync(context, output);

            string content = null;
            if (Truncate != null)
                content = Truncate.Model?.ToString();

            content ??= (await output.GetChildContentAsync(NullHtmlEncoder.Default)).GetContent(NullHtmlEncoder.Default);

            if (string.IsNullOrEmpty(content))
                return;

            if (content.Length <= Length)
            {
                output.Content.SetContent(content);
                return;
            }

            var sb = new StringBuilder();
            sb.Append("<a href=\"#\" title=\"Click to view\" class=\"modal-trigger\" data-type=\"text\">");
            sb.Append(content.Substring(0, Length));
            sb.Append(" ...");
            sb.Append("<span style=\"display: none\">");
            sb.Append(content);
            sb.Append("</span></a>");
            output.Content.SetHtmlContent(sb.ToString());
        }
    }
}
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EverFunTravel.TagHelpers
{
    [HtmlTargetElement("add-action")]
    public class AddActionTagHelper : TagHelper
    {

        [HtmlAttributeName("area")]
        public string? Area { get; set; }

        [HtmlAttributeName("controller")]
        public string? Controller { get; set; }

        [HtmlAttributeName("action")]
        public string? Action { get; set; }

        [HtmlAttributeName("text")]
        public string? Text { get; set; }

        [HtmlAttributeName("id")]
        public string? Id { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "";
            output.TagMode = TagMode.StartTagAndEndTag;
            var sbHref = new StringBuilder();
            if (!string.IsNullOrEmpty(Area))
                sbHref.Append($"/{Area}/{Controller}/{Action}/");
            else if (!string.IsNullOrEmpty(Controller))
                sbHref.Append($"{Controller}/{Action}/");
            else if (!string.IsNullOrEmpty(Action))
                sbHref.Append($"{Action}/");
            var htmlContent = $"<a data-toggle=\"modal\" data-target=\"#myModal\"  href=\"{sbHref}\" id=\"{Id}\"><img src=\"/images/icon_plus.png\" width=\"24\" /></a>";
            output.Content.SetHtmlContent(htmlContent);
            return Task.CompletedTask;
        }
    }
}



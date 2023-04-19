using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Budget.TagHelpers;

[HtmlTargetElement("select", Attributes = "asp-*")]
[HtmlTargetElement("input", Attributes = "asp-*")]
[HtmlTargetElement("textarea", Attributes = "asp-*")]
public class ChangeDataTagHelper : TagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.SetAttribute("class", "form-control text-center");
    }
}

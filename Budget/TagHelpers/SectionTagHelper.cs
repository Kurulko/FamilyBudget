using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Budget.TagHelpers;

public class SectionTagHelper : TagHelper
{
    public string Name { get; set; } = null!;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        TagBuilder sectionName = new("h3");
        sectionName.AddCssClass("text-secondary");
        sectionName.InnerHtml.Append(Name);
        output.Content.AppendHtml(sectionName);

        TagBuilder hr = new("hr");
        TagBuilder br = new("br");

        output.Content.AppendHtml(hr);

        var childContent = await output.GetChildContentAsync();
        output.Content.AppendHtml(childContent);

        output.Content.AppendHtml(hr);
        output.Content.AppendHtml(br);
    }
}

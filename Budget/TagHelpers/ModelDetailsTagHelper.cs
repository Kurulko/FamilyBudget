using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;
using System.Threading.Tasks;

namespace Budget.TagHelpers;


public class ModelDetailsTagHelper : TagHelper
{
    public ModelExpression For { get; set; } = null!;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagMode = TagMode.StartTagAndEndTag;

        TagBuilder parentDiv = new("div");
        parentDiv.AddCssClass("row");

        string name = For.Metadata.GetDisplayName() ?? For.Name;
        TagBuilder childNameDiv = GetDivColTagBuilder(GetLabelTagBuilder(name));

        parentDiv.InnerHtml.AppendHtml(childNameDiv);


        string value = For.Model?.ToString() ?? string.Empty;
        var childContent = await output.GetChildContentAsync();
        TagBuilder childValueDiv = GetDivColTagBuilder(!childContent.IsEmptyOrWhiteSpace ? childContent : GetLabelTagBuilder(value));

        parentDiv.InnerHtml.AppendHtml(childValueDiv);


        output.Content.AppendHtml(parentDiv);
    }

    TagBuilder GetLabelTagBuilder(string value)
    {
        TagBuilder label = new("label");

        string name = value;
        label.InnerHtml.Append(name);

        return label;
    }
    TagBuilder GetDivColTagBuilder(IHtmlContent childElement)
    {
        TagBuilder divCol = new("div");

        divCol.AddCssClass("col");
        divCol.InnerHtml.AppendHtml(childElement);

        return divCol;
    }
}

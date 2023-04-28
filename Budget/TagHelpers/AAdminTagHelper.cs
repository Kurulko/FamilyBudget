using Budget.Models.ViewModel.Helpers;
using Budget.Models.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using Budget.Models.Database;

namespace Budget.TagHelpers;

[HtmlTargetElement("a", Attributes = "model")]
public class AAdminTagHelper : TagHelper
{
    public TagAModel Model { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        TagBuilder a = new("a");

        string href = Model.GetHref();
        a.Attributes.Add("href", href);

        string color = ModeHelper.GetBootstrapColorByMode(Model.Mode);
        a.AddCssClass($"btn btn-outline-{color}");

        var child = await output.GetChildContentAsync();
        if(child?.IsEmptyOrWhiteSpace ?? true)
            a.InnerHtml.Append(Model.Mode.ToString());
        else
            a.InnerHtml.AppendHtml(child!);

        output.Content.AppendHtml(a);
    }
}


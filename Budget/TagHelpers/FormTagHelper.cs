using Budget.Models.ViewModel;
using Budget.Models.ViewModel.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Budget.TagHelpers;

public class FormValidationTagHelper : TagHelper
{
    public TagAModel FormModel { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        TagBuilder form = new("form");

        form.AddCssClass("form-group");
        form.Attributes.Add("method", "post");
        form.Attributes.Add("action", FormModel.GetHref());

        var childContent = await output.GetChildContentAsync();
        form.InnerHtml.AppendHtml(childContent);

        Mode mode = FormModel.Mode;
        if (mode != Mode.Get)
        {
            TagBuilder submit = new("input");

            string color = ModeHelper.GetBootstrapColorByMode(mode);
            submit.Attributes.Add("type", "submit");
            submit.Attributes.Add("value", mode.ToString());
            submit.AddCssClass($"btn btn-outline-{color}");

            form.InnerHtml.AppendHtml(submit);
        }

        output.Content.AppendHtml(form);
    }
}

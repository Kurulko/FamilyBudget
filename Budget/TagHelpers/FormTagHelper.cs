using Budget.Models.ViewModel;
using Budget.Models.ViewModel.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Budget.TagHelpers;

public class FormValidationTagHelper : TagHelper
{
    public FormModel FormModel { get; set; } = null!;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        TagBuilder form = new("form");

        form.AddCssClass("form-group");
        form.Attributes.Add("method", "post");
        form.Attributes.Add("action", FormModel.Action);

        TagBuilder divVal = new("div");

        divVal.AddCssClass("text-danger");
        divVal.Attributes.Add("style", "font-style: oblique;");
        divVal.Attributes.Add("asp-validation-summary", "ModelOnly");

        form.InnerHtml.AppendHtml(divVal);

        var childContent = await output.GetChildContentAsync();
        form.InnerHtml.AppendHtml(childContent);

        Mode mode = FormModel.Mode;
        if (mode != Mode.Get)
        {
            TagBuilder submit = new("input");

            string color = ModeHelper.GetBootstrapColorByMode(mode);
            submit.AddCssClass($"btn btn-outline-{color}");

            submit.Attributes.Add("value", mode.ToString());
            submit.Attributes.Add("type", "submit");

            form.InnerHtml.AppendHtml(submit);
        }

        output.Content.AppendHtml(form);
    }
}

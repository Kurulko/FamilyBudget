using System.ComponentModel.DataAnnotations;

namespace Budget.Attributes;

public class RequiredEnter : RequiredAttribute
{
    readonly string? name;
    public RequiredEnter(string? name = null)  
        => this.name = name;

    public override bool IsValid(object? value)
    {
        bool result = base.IsValid(value);
        if (!result && string.IsNullOrEmpty(ErrorMessage))
            ErrorMessage = "Enter your " + (string.IsNullOrEmpty(name) ? "{0}" : name);
        return result;
    }
}

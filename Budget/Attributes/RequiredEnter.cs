using System.ComponentModel.DataAnnotations;

namespace Budget.Attributes;

public class RequiredEnter : RequiredAttribute
{
    public RequiredEnter(string? name = null)
        => ErrorMessage = "Enter your " +  name is null ? "{0}" : name;
}

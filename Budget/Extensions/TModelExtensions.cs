namespace Budget.Extensions;

public static class TModelExtensions
{
    public static string ToStringAndLower<TModel>(this TModel model)
        => model!.ToString()!.ToLower();
}

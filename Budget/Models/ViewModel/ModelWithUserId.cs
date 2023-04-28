namespace Budget.Models.ViewModel;

public record ModelWithUserId<TModel>(TModel Model, string? UserId);

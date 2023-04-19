using Budget.Models.ViewModel.Helpers;

namespace Budget.Models.ViewModel;

public record ModelWithMode<TModel>(TModel Model, Mode Mode);

using Budget.Models.Database;

namespace Budget.Models.ViewModel;

public record ModelWithTypeOfOperation<TModel>(TModel Model, TypeOfOperation TypeOfOperation);

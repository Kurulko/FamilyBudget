using Budget.Models.Database;

namespace Budget.Models.ViewModel;

public record GroupMoney(TypeOfMoney TypeOfMoney, char CurrencySymbol, decimal SumOfMoney);

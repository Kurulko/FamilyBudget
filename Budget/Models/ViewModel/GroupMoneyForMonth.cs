using Budget.Models.Database;
using System.Collections.Generic;

namespace Budget.Models.ViewModel;

public record GroupMoneyForMonth(TypeOfMoney TypeOfMoney, char CurrencySymbol, decimal SumOfMoney, MonthAndYear MonthAndYear, IEnumerable<TypeOfOperation> TypesOfOperation);

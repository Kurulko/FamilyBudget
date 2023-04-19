using Budget.Models.Database;

namespace Budget.Extensions;

public static class TypeOfOperationExtensions
{
    public static bool TryParse(string typeOfOperationStr, out TypeOfOperation typeOfOperation)
    {
        bool result = true;

        string typeOfOperationStrLower = typeOfOperationStr.ToLower();

        if (typeOfOperationStrLower == TypeOfOperation.Purchase.ToStringAndLower())
            typeOfOperation = TypeOfOperation.Purchase;
        else if (typeOfOperationStrLower == TypeOfOperation.Receiving.ToStringAndLower())
            typeOfOperation = TypeOfOperation.Receiving;
        else
        {
            result = false;
            typeOfOperation = default;
        }

        return result;
    }
}

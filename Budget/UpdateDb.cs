using Budget.Models.Database;
using System;
using System.Linq;

namespace Budget;

public class Updatedb
{
    public static void UpdateMoneyForEveryone(BudgetContext db)
    {
        var everyone = db.MoneyForEveryone;
        decimal sum = everyone.Sum(m => m.Paid);
        decimal forEveryone = Math.Round(sum / everyone.Count(), 2);
        foreach (var item in everyone)
        {
            if (item.Paid > forEveryone)
            {
                item.MustGet = Math.Round(item.Paid - forEveryone, 2);
                item.MustPay = 0.0m;
            }
            else if (item.Paid < forEveryone)
            {
                item.MustGet = 0.0m;
                item.MustPay = Math.Round(forEveryone - item.Paid, 2);
            }
            else
            {
                item.MustGet = 0.0m;
                item.MustPay = 0.0m;
            }
            item.MiddleValue = forEveryone;
        }
        db.SaveChanges();
    }
}

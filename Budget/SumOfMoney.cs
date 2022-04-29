using Budget.Models;
using System.Collections.Generic;

namespace Budget
{
    public class SumOfMoney
    {
        public static void SumOfMoneyThisPerson(BudgetContext db, User person)
        {
            if (person != null)
            {
                decimal sumOfCash = 0.0m, sumOfCart = 0.0m;
                List<SpendMoney> purchases = person.Spend;
                foreach (SpendMoney purchase in purchases)
                {
                    if (purchase.IsCash)
                        sumOfCash += purchase.Money;
                    else
                        sumOfCart += purchase.Money;
                }
                person.SpendCash = sumOfCash;
                person.SpendMoneyFromTheCard = sumOfCart;
                db.Users.Update(person);
                db.SaveChanges();
            }
        }
    }
}

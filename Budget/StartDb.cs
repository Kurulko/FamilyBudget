using Budget.Models;
using System.Collections.Generic;
using System.Linq;

namespace Budget;

public static class StartDb
{
    public static void Initialize(BudgetContext db)
    {
        if(!db.Users.Any())
        {
            List<User> people = new List<User>()
            {
                new User(){UserName = "Тетя Галя"},
                new User(){UserName = "Света"},
                new User(){UserName = "Настя"},
                new User(){UserName = "Кирилл"},
                new User(){UserName = "Карина, Серега"},
                new User(){UserName = "Поля"}
            };

            foreach (User person in people)
            {
                db.Users.Add(person);
                db.SaveChanges();
            }
        }
    }
}

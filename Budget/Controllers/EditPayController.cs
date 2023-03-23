using Budget.Models.Database;
using Budget.Models.ViewModel;
using Budget.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Controllers;

public class EditPayController : EditController<Operation, long>
{
    public EditPayController(IUserService userService, BudgetContext db) : base(userService, db) { }

    public override Task<IActionResult> AddAsync()
        => Task.FromResult(View(new  GetMoney() { Money = 0.0m, Time = DateTime.Now }));

    public override async Task<IActionResult> AddAsync(Operation operation)
    {
        if (ModelState.IsValid)
        {
            User person = (await userService.GetUserByClaimsAsync(User))!;
            if (person != null)
            {
                person.Get.Add(getMoney);
                if (getMoney.IsCash)
                    person.NowMoneyInCash += getMoney.Money;
                else
                    person.NowMoneyInCart += getMoney.Money;

                db.SaveChanges();
                return RedirectToAction("Pay", "Person");
            }
        }
        return View(getMoney);
    }

    public override async Task<IActionResult> EditAsync(long id)
    {
        string personId = userManager.GetUserId(User);
        User person = db.Users.Include(p => p.Get)
            .FirstOrDefault(p => p.Id == personId);
        if (person != null)
        {
            GetMoney get = person.Get.FirstOrDefault(p => p.Id == getId);
            if (get != null)
                return View(get);
            return RedirectToAction("Pay", "Person");
        }
        return RedirectToAction("Person", "Person");
    }
    public override async Task<IActionResult> EditAsync(GetMoney wasGetMoney)
    {
        if (ModelState.IsValid)
        {
            string personId = userManager.GetUserId(User);
            User person = db.Users.Include(p => p.Get)
                .FirstOrDefault(p => p.Id == personId);
            if (person != null)
            {
                GetMoney get = person.Get.FirstOrDefault(p => p.Id == wasGetMoney.Id);
                if (get != null)
                {
                    if (wasGetMoney.IsCash)
                    {
                        if (get.IsCash)
                            person.NowMoneyInCash += wasGetMoney.Money - get.Money;
                        else
                        {
                            person.NowMoneyInCash += wasGetMoney.Money;
                            person.NowMoneyInCart -= get.Money;
                        }
                    }
                    else
                    {
                        if (!get.IsCash)
                            person.NowMoneyInCart += wasGetMoney.Money - get.Money;
                        else
                        {
                            person.NowMoneyInCash -= get.Money;
                            person.NowMoneyInCart += wasGetMoney.Money;
                        }
                    }

                    get.Name = wasGetMoney.Name;
                    get.Money = wasGetMoney.Money;
                    get.Time = wasGetMoney.Time;
                    get.IsCash = wasGetMoney.IsCash;

                    db.SaveChanges();
                    return RedirectToAction("Pay", "Person");
                }
            }
        }
        return View(wasGetMoney);
    }


    public override async Task<IActionResult> DeleteAsync(long id)
    {
        string personId = userManager.GetUserId(User);
        User person = db.Users.Include(p => p.Get)
            .FirstOrDefault(p => p.Id == personId);
        if (person != null)
        {
            GetMoney get = person.Get.FirstOrDefault(p => p.Id == getId);
            if (get != null)
            {
                db.GetMoney.Remove(get);
                if (get.IsCash)
                    person.NowMoneyInCash -= get.Money;
                else
                    person.NowMoneyInCart -= get.Money;

                db.SaveChanges();
                return RedirectToAction("Pay", "Person");
            }
        }
        return RedirectToAction("Person", "Person");
    }
}

using Budget.Models;
using Budget.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Budget.Controllers;

[Authorize]
public class EditPayController : Controller
{
    public BudgetContext Db { get; set; }
    public UserManager<User> UserManager { get; set; }

    public EditPayController(BudgetContext context,
        UserManager<User> userManager)
    {
        Db = context;
        UserManager = userManager;
    }

    [HttpGet]
    public IActionResult Add()
    {
        string personId = UserManager.GetUserId(User);
        User person = Db.Users.FirstOrDefault(p => p.Id == personId);
        if (person != null)
            return View(new  GetMoney() { Money = 0.0m, Time = DateTime.Now });
        return RedirectToAction("Person", "Person");
    }
    [HttpPost]
    public IActionResult Add(GetMoney getMoney)
    {
        if (ModelState.IsValid)
        {
            string personId = UserManager.GetUserId(User);
            User person = Db.Users.Include(p => p.Get)
                .FirstOrDefault(p => p.Id == personId);
            if (person != null)
            {
                person.Get.Add(getMoney);
                if (getMoney.IsCash)
                    person.NowMoneyInCash += getMoney.Money;
                else
                    person.NowMoneyInCart += getMoney.Money;

                Db.SaveChanges();
                return RedirectToAction("Pay", "Person");
            }
        }
        return View(getMoney);
    }

    [HttpGet]
    public IActionResult Edit(int getId)
    {
        string personId = UserManager.GetUserId(User);
        User person = Db.Users.Include(p => p.Get)
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
    [HttpPost]
    public IActionResult Edit(GetMoney wasGetMoney)
    {
        if (ModelState.IsValid)
        {
            string personId = UserManager.GetUserId(User);
            User person = Db.Users.Include(p => p.Get)
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

                    Db.SaveChanges();
                    return RedirectToAction("Pay", "Person");
                }
            }
        }
        return View(wasGetMoney);
    }


    [HttpGet]
    public IActionResult Delete(int getId)
    {
        string personId = UserManager.GetUserId(User);
        User person = Db.Users.Include(p => p.Get)
            .FirstOrDefault(p => p.Id == personId);
        if (person != null)
        {
            GetMoney get = person.Get.FirstOrDefault(p => p.Id == getId);
            if (get != null)
            {
                Db.GetMoney.Remove(get);
                if (get.IsCash)
                    person.NowMoneyInCash -= get.Money;
                else
                    person.NowMoneyInCart -= get.Money;

                Db.SaveChanges();
                return RedirectToAction("Pay", "Person");
            }
        }
        return RedirectToAction("Person", "Person");
    }
}

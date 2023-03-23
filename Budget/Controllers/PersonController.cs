using Budget.Models.Database;
using Budget.Models.ViewModel;
using Budget.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Budget.Controllers;

[Authorize]
public class PersonController : BudgetController
{
    readonly BudgetContext db;
    public PersonController(IUserService userService, BudgetContext db) : base(userService)
        => this.db = db;

    public IActionResult Person()
    {
        string id = userManager.GetUserId(User);
        User person = db.Users.Include(p => p.Spend)
            .FirstOrDefault(p => p.Id == id);
        if (person != null)
            return View(person.UserName as object);
        return RedirectToAction("Index","Home");
    }

    public IActionResult Purchases()
    {
        string id = userManager.GetUserId(User);
        User person = db.Users.Include(p => p.Spend)
            .FirstOrDefault(p => p.Id == id);
        db.SaveChanges();
        if (person != null)
            return View(person);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Pay()
    {
        string id = userManager.GetUserId(User);
        User person = db.Users.Include(p => p.Get)
            .Include(p => p.Spend)
            .FirstOrDefault(p => p.Id == id);
        if (person != null)
            return View(new GetAndPerson { Get = person.Get, User = person });
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Compare()
    {
        string id = userManager.GetUserId(User);
        User person = db.Users.Include(p => p.Get)
            .Include(p => p.Spend)
            .FirstOrDefault(p => p.Id == id);
        if (person != null)
            return View(new GetMoneyAndSpendMoneyAndPerson
            {
                Get = person.Get,
                Spend = person.Spend,
                PersonId = person.Id,
                NowMoneyInCart = person.NowMoneyInCart,
                NowMoneyInCash = person.NowMoneyInCash
            }) ;
        return RedirectToAction("Index", "Home");
    }
}

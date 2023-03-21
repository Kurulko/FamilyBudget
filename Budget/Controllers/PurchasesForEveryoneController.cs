using Budget.Models;
using Budget.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Budget.Controllers;

public class PurchasesForEveryoneController : Controller
{
    public BudgetContext Db { get; set; }
    public PurchasesForEveryoneController(BudgetContext context)
       => Db = context;

    public IActionResult Result()
    {
        var moneyAndPerson = Db.MoneyForEveryone
            .Include(m => m.Person).Select(m => new MoneyForEveryoneAndPerson { PersonId = m.PersonId, Money = m, 
                Name = m.Person.UserName }).ToList();
        return View(moneyAndPerson);
    }

    [HttpGet]
    public IActionResult Delete()
    {
        Db.MoneyForEveryone.RemoveRange(Db.MoneyForEveryone);
        Db.SaveChanges();
        return RedirectToAction("Result");
    }

    [HttpGet]
    public IActionResult Add()
    {
        ViewBag.Names = Db.Users.Select(p => new NameAndId { Id = p.Id, Name = p.UserName }).ToList();
        return View();
    }
    [HttpPost]
    public IActionResult Add(MoneyForEveryone value)
    {
        if (ModelState.IsValid)
        {
            User person = Db.Users.FirstOrDefault(p => p.Id == value.PersonId);
            if (person != null)
            {
                var money = Db.MoneyForEveryone.FirstOrDefault(m => m.PersonId == value.PersonId);
                if (money == null)
                    Db.MoneyForEveryone.Add(value);
                else
                    money.Paid += value.Paid;
                Db.SaveChanges();

                UpdateDb.UpdateMoneyForEveryone(Db);
                return RedirectToAction("Result");
            }
        }
        ViewBag.Names = Db.Users.Select(p => new NameAndId { Id = p.Id, Name = p.UserName }).ToList();
        return View(value);
    }
}

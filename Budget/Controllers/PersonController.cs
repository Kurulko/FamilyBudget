using Budget.Models;
using Budget.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Budget.Controllers
{
    [Authorize]
    public class PersonController : Controller
    {
        public BudgetContext Db { get; set; }
        public UserManager<User> UserManager { get; set; }

        public PersonController(BudgetContext context,
            UserManager<User> userManager)
        {
            Db = context;
            UserManager = userManager;
        }

        public IActionResult Person()
        {
            string id = UserManager.GetUserId(User);
            User person = Db.Users.Include(p => p.Spend)
                .FirstOrDefault(p => p.Id == id);
            if (person != null)
                return View(person.UserName as object);
            return RedirectToAction("Index","Home");
        }

        public IActionResult Purchases()
        {
            string id = UserManager.GetUserId(User);
            User person = Db.Users.Include(p => p.Spend)
                .FirstOrDefault(p => p.Id == id);
            Db.SaveChanges();
            if (person != null)
                return View(person);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Pay()
        {
            string id = UserManager.GetUserId(User);
            User person = Db.Users.Include(p => p.Get)
                .Include(p => p.Spend)
                .FirstOrDefault(p => p.Id == id);
            if (person != null)
                return View(new GetAndPerson { Get = person.Get, User = person });
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Compare()
        {
            string id = UserManager.GetUserId(User);
            User person = Db.Users.Include(p => p.Get)
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
}

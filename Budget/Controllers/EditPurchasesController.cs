using Budget.Models.Database;
using Budget.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Budget.Controllers;

public class EditPurchasesController : EditController<SpendMoney, int>
{

    public EditPurchasesController(BudgetContext db,
        UserManager<User> userManager) : base(userManager, db) { }


    [HttpGet]
    public override IActionResult Add()
    {
        string personId = userManager.GetUserId(User);
        User person = db.Users.FirstOrDefault(p => p.Id == personId);
        if (person != null)
            return View(new SpendMoney() { Money = 0.0m, Time = DateTime.Now });
        return RedirectToAction("ListOfPeople", "People");
    }
    [HttpPost]
    public override IActionResult Add(SpendMoney nowPurchase)
    {
        if (ModelState.IsValid)
        {
            string personId = userManager.GetUserId(User);
            User person = db.Users.Include(p => p.Spend)
                .Include(p => p.Spend)
                .FirstOrDefault(p => p.Id == personId);
            if (person != null)
            {
                if (nowPurchase.IsCash)
                {
                    if (person.NowMoneyInCash >= nowPurchase.Money)
                    {
                        person.Spend.Add(nowPurchase);

                        SpendMoney lastPurchase = person.Spend.LastOrDefault();

                        decimal nowMoney = person.NowMoneyInCash - nowPurchase.Money;
                        lastPurchase.WasMoney = $"{person.NowMoneyInCash} - {lastPurchase.Money} = {nowMoney}";
                        person.NowMoneyInCash = nowMoney;
                        db.SaveChanges();

                        SumOfMoney.SumOfMoneyThisPerson(db, person);
                        return RedirectToAction("Purchases", "Person", new { id = nowPurchase.PersonId });
                    }
                    else
                        ModelState.AddModelError("", "Недостаточно средств");
                }
                else
                {
                    if (person.NowMoneyInCart >= nowPurchase.Money)
                    {
                        person.Spend.Add(nowPurchase);

                        SpendMoney lastPurchase = person.Spend.LastOrDefault();

                        decimal nowMoney = person.NowMoneyInCart - nowPurchase.Money;
                        lastPurchase.WasMoney = $"{person.NowMoneyInCart} - {lastPurchase.Money} = {nowMoney}";
                        person.NowMoneyInCart = nowMoney;
                        db.SaveChanges();

                        SumOfMoney.SumOfMoneyThisPerson(db, person);
                        return RedirectToAction("Purchases", "Person");
                    }
                    else
                        ModelState.AddModelError("", "Недостаточно средств");
                }
            }
        }
        return View(nowPurchase);
    }

    [HttpGet]
    public override IActionResult Edit(int purchaseId)
    {
        string personId = userManager.GetUserId(User);
        User person = db.Users.Include(p => p.Spend)
            .FirstOrDefault(p => p.Id == personId);
        if (person != null)
        {
            SpendMoney purchase = person.Spend.FirstOrDefault(p => p.Id == purchaseId);
            if (purchase != null)
                return View(purchase);
            return RedirectToAction("Purchases", "Person", new { id = personId });
        }
        return RedirectToAction("ListOfPeople", "People");
    }
    [HttpPost]
    public override IActionResult Edit(SpendMoney nowPurchase)
    {
        if (ModelState.IsValid)
        {
            string personId = userManager.GetUserId(User);
            User person = db.Users.Include(p => p.Spend)
                .FirstOrDefault(p => p.Id == personId);
            if (person != null)
            {
                SpendMoney wasPurchase = person.Spend.FirstOrDefault(p => p.Id == nowPurchase.Id);
                if (wasPurchase != null)
                {
                    if (nowPurchase.IsCash)
                    {
                        if (wasPurchase.IsCash)
                        {
                            //был кеш и он и остался
                            if (person.NowMoneyInCash + wasPurchase.Money - nowPurchase.Money >= 0)
                            {
                                decimal nowMoney = person.NowMoneyInCash + wasPurchase.Money - nowPurchase.Money;
                                wasPurchase.WasMoney = $"{person.NowMoneyInCash + wasPurchase.Money}" +
                                    $" - {nowPurchase.Money} = {nowMoney}";

                                wasPurchase.IsCash = nowPurchase.IsCash;
                                wasPurchase.Name = nowPurchase.Name;
                                wasPurchase.Money = nowPurchase.Money;
                                wasPurchase.Time = nowPurchase.Time;

                                person.NowMoneyInCash = nowMoney;
                                db.SaveChanges();

                                SumOfMoney.SumOfMoneyThisPerson(db, person);
                                return RedirectToAction("Purchases", "Person", new { id = nowPurchase.PersonId });
                            }
                            else
                                ModelState.AddModelError("", "Недостаточно средств");
                        }
                        else
                        {
                            //была карта, а теперь кеш
                            if (person.NowMoneyInCash - nowPurchase.Money >= 0)
                            {
                                decimal nowMoneyInCart = person.NowMoneyInCart + wasPurchase.Money;
                                decimal nowMoneyInCash = person.NowMoneyInCash - nowPurchase.Money;
                                wasPurchase.WasMoney = $"{person.NowMoneyInCash}" +
                                    $" - {nowPurchase.Money} = {nowMoneyInCash}";

                                wasPurchase.IsCash = nowPurchase.IsCash;
                                wasPurchase.Name = nowPurchase.Name;
                                wasPurchase.Money = nowPurchase.Money;
                                wasPurchase.Time = nowPurchase.Time;

                                person.NowMoneyInCart = nowMoneyInCart;
                                person.NowMoneyInCash = nowMoneyInCash;
                                db.SaveChanges();

                                SumOfMoney.SumOfMoneyThisPerson(db, person);
                                return RedirectToAction("Purchases", "Person");
                            }
                            else
                                ModelState.AddModelError("", "Недостаточно средств");
                        }
                    }
                    else
                    {
                        if (!wasPurchase.IsCash)
                        {
                            //была карта и она и осталась
                            if (person.NowMoneyInCart + wasPurchase.Money - nowPurchase.Money >= 0)
                            {
                                decimal nowMoney = person.NowMoneyInCart + wasPurchase.Money - nowPurchase.Money;
                                wasPurchase.WasMoney = $"{person.NowMoneyInCart + wasPurchase.Money}" +
                                    $" - {nowPurchase.Money} = {nowMoney}";

                                wasPurchase.IsCash = nowPurchase.IsCash;
                                wasPurchase.Name = nowPurchase.Name;
                                wasPurchase.Money = nowPurchase.Money;
                                wasPurchase.Time = nowPurchase.Time;

                                person.NowMoneyInCart = nowMoney;
                                db.SaveChanges();

                                SumOfMoney.SumOfMoneyThisPerson(db, person);
                                return RedirectToAction("Purchases", "Person");
                            }
                            else
                                ModelState.AddModelError("", "Недостаточно средств");
                        }
                        else
                        {
                            //был кеш, а теперь карта
                            if (person.NowMoneyInCart - nowPurchase.Money >= 0)
                            {
                                decimal nowMoneyInCart = person.NowMoneyInCart - nowPurchase.Money;
                                decimal nowMoneyInCash = person.NowMoneyInCash + wasPurchase.Money;
                                wasPurchase.WasMoney = $"{person.NowMoneyInCart}" +
                                    $" - {nowPurchase.Money} = {nowMoneyInCart}";

                                wasPurchase.IsCash = nowPurchase.IsCash;
                                wasPurchase.Name = nowPurchase.Name;
                                wasPurchase.Money = nowPurchase.Money;
                                wasPurchase.Time = nowPurchase.Time;

                                person.NowMoneyInCart = nowMoneyInCart;
                                person.NowMoneyInCash = nowMoneyInCash;
                                db.SaveChanges();

                                SumOfMoney.SumOfMoneyThisPerson(db, person);
                                return RedirectToAction("Purchases", "Person");
                            }
                            else
                                ModelState.AddModelError("", "Недостаточно средств");
                        }

                    }

                }
            }
        }
        return View(nowPurchase);
    }


    [HttpGet]
    public override IActionResult Delete(int purchaseId)
    {
        string personId = userManager.GetUserId(User);
        User person = db.Users.Include(p => p.Spend)
            .FirstOrDefault(p => p.Id == personId);
        if (person != null)
        {
            SpendMoney purchase = person.Spend.FirstOrDefault(p => p.Id == purchaseId);
            if (purchase != null)
            {
                db.SpendMoney.Remove(purchase);
                if (purchase.IsCash)
                    person.NowMoneyInCash += purchase.Money;
                else
                    person.NowMoneyInCart += purchase.Money;
                db.SaveChanges();
                SumOfMoney.SumOfMoneyThisPerson(db, person);
                return RedirectToAction("Purchases", "Person");
            }
        }
        return RedirectToAction("ListOfPeople", "People");
    }
}

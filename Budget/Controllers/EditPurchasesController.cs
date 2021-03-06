using Budget.Models;
using Budget.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Budget.Controllers
{
    [Authorize]
    public class EditPurchasesController : Controller
    {
        public BudgetContext Db { get; set; }
        public UserManager<User> UserManager { get; set; }

        public EditPurchasesController(BudgetContext context,
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
                return View(new SpendMoney() { Money = 0.0m, Time = DateTime.Now });
            return RedirectToAction("ListOfPeople", "People");
        }
        [HttpPost]
        public IActionResult Add(SpendMoney nowPurchase)
        {
            if (ModelState.IsValid)
            {
                string personId = UserManager.GetUserId(User);
                User person = Db.Users.Include(p => p.Spend)
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
                            Db.SaveChanges();

                            SumOfMoney.SumOfMoneyThisPerson(Db, person);
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
                            Db.SaveChanges();

                            SumOfMoney.SumOfMoneyThisPerson(Db, person);
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
        public IActionResult Edit(int purchaseId)
        {
            string personId = UserManager.GetUserId(User);
            User person = Db.Users.Include(p => p.Spend)
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
        public IActionResult Edit(SpendMoney nowPurchase)
        {
            if (ModelState.IsValid)
            {
                string personId = UserManager.GetUserId(User);
                User person = Db.Users.Include(p => p.Spend)
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
                                    Db.SaveChanges();

                                    SumOfMoney.SumOfMoneyThisPerson(Db, person);
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
                                    Db.SaveChanges();

                                    SumOfMoney.SumOfMoneyThisPerson(Db, person);
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
                                    Db.SaveChanges();

                                    SumOfMoney.SumOfMoneyThisPerson(Db, person);
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
                                    Db.SaveChanges();

                                    SumOfMoney.SumOfMoneyThisPerson(Db, person);
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
        public IActionResult Delete(int purchaseId)
        {
            string personId = UserManager.GetUserId(User);
            User person = Db.Users.Include(p => p.Spend)
                .FirstOrDefault(p => p.Id == personId);
            if (person != null)
            {
                SpendMoney purchase = person.Spend.FirstOrDefault(p => p.Id == purchaseId);
                if (purchase != null)
                {
                    Db.SpendMoney.Remove(purchase);
                    if (purchase.IsCash)
                        person.NowMoneyInCash += purchase.Money;
                    else
                        person.NowMoneyInCart += purchase.Money;
                    Db.SaveChanges();
                    SumOfMoney.SumOfMoneyThisPerson(Db, person);
                    return RedirectToAction("Purchases", "Person");
                }
            }
            return RedirectToAction("ListOfPeople", "People");
        }
    }
}

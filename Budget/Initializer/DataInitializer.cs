using Budget.Models.Database;
using Budget.Services.Db;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Initializer;

public class DataInitializer
{
    public static async Task InitializeAsync(string userId, DbModelService<Operation> operationService, DbModelService<Category> categoryService, DbModelService<Money> moneyService, DbModelService<Currency> currencyService)
    {
        List<Currency> currencies = new(){
            new() {/* Id = 1, */ShortName = "UAN", FullName = "grivna", Symbol = '₴', UserId = userId},
            new() {/* Id = 2, */ShortName = "DOL", FullName = "dollar", Symbol = '$', UserId = userId},
            new() {/* Id = 3, */ShortName = "EURO", FullName = "euro", Symbol = '€', UserId = userId}
        };
        await currencyService.AddRangeModelsAsync(currencies);

        List<Category> categories = new(){
            new() {/* Id = 1, */Name = "Food" , UserId = userId},
            new() {/* Id = 2, */Name = "Pets", UserId = userId},
            new() {/* Id = 3, */Name = "Others", UserId = userId},
            new() {/* Id = 4, */Name = "Techin", UserId = userId},
            new() {/* Id = 5, */Name = "Work", UserId = userId},
            new() {/* Id = 6, */Name = "University", UserId = userId},
        };
        await categoryService.AddRangeModelsAsync(categories);

        List<Category> childCategories = new(){
            new() {/* Id = 4, */Name = "KFC", ParentCategoryId = 1, UserId = userId },
            new() {/* Id = 5, */Name = "Sushi" , ParentCategoryId = 1, UserId = userId },
            new() {/* Id = 5, */Name = "PC" , ParentCategoryId = 4, UserId = userId },
        };
        await categoryService.AddRangeModelsAsync(childCategories);

        List<Money> money = new(){
            new() {/* Id = 1,*/ Count = 1500, TypeOfMoney = TypeOfMoney.Card, CurrencyId = 1, UserId = userId},
            new() {/* Id = 2,*/ Count = 10, TypeOfMoney = TypeOfMoney.Cash, CurrencyId = 1, UserId = userId},
            new() {/* Id = 3,*/ Count = 540, TypeOfMoney = TypeOfMoney.Card, CurrencyId = 2, UserId = userId},
            new() {/* Id = 4,*/ Count = 79400, TypeOfMoney = TypeOfMoney.Cash, CurrencyId = 2, UserId = userId},
            new() {/* Id = 5,*/ Count = 468, TypeOfMoney = TypeOfMoney.Cash, CurrencyId = 2, UserId = userId},
            new() {/* Id = 6,*/ Count = 5435, TypeOfMoney = TypeOfMoney.Card, CurrencyId = 3 , UserId = userId},
            new() {/* Id = 7,*/ Count = 4565, TypeOfMoney = TypeOfMoney.Card, CurrencyId = 3 , UserId = userId},
            new() {/* Id = 8,*/ Count = 976, TypeOfMoney = TypeOfMoney.Cash, CurrencyId = 3 , UserId = userId},
            new() {/* Id = 9,*/ Count = 675757, TypeOfMoney = TypeOfMoney.Cash, CurrencyId = 3 , UserId = userId},
            new() {/* Id = 10,*/ Count = 679, TypeOfMoney = TypeOfMoney.Card, CurrencyId = 2 , UserId = userId},
            new() {/* Id = 11,*/ Count = 932476, TypeOfMoney = TypeOfMoney.Cash, CurrencyId = 1 , UserId = userId},
        };
        await moneyService.AddRangeModelsAsync(money);

        List<Operation> operations = new() {
            new() {/* Id = 1,*/ Name = "Monitor", CategoryId = categories[2].Id, MoneyId = money[0].Id, UserId = userId},
            new() {/* Id = 2,*/ Name = "Dog", CategoryId = categories[1].Id, MoneyId = money[2].Id, UserId = userId},
            new() {/* Id = 3,*/ Name = "Cat", CategoryId = categories[0].Id, MoneyId = money[1].Id, UserId = userId, DateTime = DateTime.Now.AddMonths(-2)},
            new() {/* Id = 4,*/ Name = "PC", CategoryId = categories[2].Id, MoneyId = money[3].Id, UserId = userId, DateTime = DateTime.Now.AddMonths(-2)},
            new() {/* Id = 5,*/ Name = "HZ1", CategoryId = categories[3].Id, MoneyId = money[5].Id, UserId = userId, DateTime = DateTime.Now.AddMonths(-5)},
            new() {/* Id = 6,*/ Name = "HZ2", CategoryId = categories[3].Id, MoneyId = money[4].Id, UserId = userId, DateTime = DateTime.Now.AddMonths(-13)},
            new() {/* Id = 7,*/ Name = "Something", CategoryId = categories[0].Id, MoneyId = money[6].Id, UserId = userId, TypeOfOperation = TypeOfOperation.Receiving},
            new() {/* Id = 8,*/ Name = "Salary", CategoryId = categories[5].Id, MoneyId = money[9].Id, UserId = userId, TypeOfOperation = TypeOfOperation.Receiving},
            new() {/* Id = 9,*/ Name = "Scholership", CategoryId = categories[4].Id, MoneyId = money[8].Id, UserId = userId, TypeOfOperation = TypeOfOperation.Receiving},
            new() {/* Id = 10,*/ Name = "Another income", CategoryId = categories[5].Id, MoneyId = money[10].Id, UserId = userId, TypeOfOperation = TypeOfOperation.Receiving},
        };
        await operationService.AddRangeModelsAsync(operations);
    }
}

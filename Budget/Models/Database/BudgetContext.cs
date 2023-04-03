using Budget.Controllers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Budget.Models.Database;

public class BudgetContext : IdentityDbContext<User>
{
    public DbSet<Money> Money { get; set; } = null!;
    public DbSet<Currency> Currencies { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Operation> Operations { get; set; } = null!;

    public BudgetContext(DbContextOptions<BudgetContext> options) : base(options)
        => Database.EnsureCreated();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        //builder.Entity<Category>().HasMany(c => c.ChildCategories).WithOne(sc => sc.ParentCategory);

        List<Currency> currencies = new(){
            new() { Id = 1, ShortName = "UAN", FullName = "grivna"},
            new() { Id = 2, ShortName = "DOL", FullName = "dollar"}
        };
        builder.Entity<Currency>().HasData(currencies);

        List<Category> categories = new(){
            new() { Id = 1, Name = "Food" },
            new() { Id = 2, Name = "Pets"},
            new() { Id = 3, Name = "Others"},
            new() { Id = 4, Name = "KFC", ParentCategoryId = 1 },
            new() { Id = 5, Name = "Sushi" , ParentCategoryId = 1 },
        };
        builder.Entity<Category>().HasData(categories);

        List<Money> money = new(){
            new() { Id = 1, Count = 1500, IsCard = true, CurrencyId = 1},
            new() { Id = 2, Count = 10, IsCard = false, CurrencyId = 1},
            new() { Id = 3, Count = 540, IsCard = true, CurrencyId = 2},
            new() { Id = 4, Count = 79400, IsCard = false, CurrencyId = 2},
        };
        builder.Entity<Money>().HasData(money);

        List<Operation> operations = new(){
            new() { Id = 1, Name = "Monitor", CategoryId = categories[2].Id, MoneyId = money[0].Id},
            new() { Id = 2, Name = "Dog", CategoryId = categories[1].Id, MoneyId = money[2].Id},
            new() { Id = 3, Name = "Cat", CategoryId = categories[1].Id, MoneyId = money[1].Id},
            new() { Id = 4, Name = "PC", CategoryId = categories[2].Id, MoneyId = money[3].Id},
        };
        builder.Entity<Operation>().HasData(operations);

        base.OnModelCreating(builder);
    }
}

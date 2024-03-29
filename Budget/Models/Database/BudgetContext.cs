﻿using Budget.Controllers;
using Budget.Services.Db;
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
        builder.Entity<Currency>().HasIndex(c => c.Symbol).IsUnique();
        builder.Entity<Currency>().HasIndex(c => c.FullName).IsUnique();

        builder.Entity<Operation>()
            .HasOne(c => c.Money)
            .WithOne(m => m.Operation)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Operation>()
            .HasOne(c => c.Category)
            .WithMany(c => c.Operations)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Category>()
            .HasMany(c => c.ChildCategories)
            .WithOne(sc => sc.ParentCategory)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.NoAction);

        List<Currency> currencies = new(){
            new() { Id = 1, ShortName = "UAN", FullName = "grivna", Symbol = '₴'},
            new() { Id = 2, ShortName = "DOL", FullName = "dollar", Symbol = '$'},
            new() { Id = 3, ShortName = "EURO", FullName = "euro", Symbol = '€'}
        };
        builder.Entity<Currency>().HasData(currencies);


        base.OnModelCreating(builder);
    }
}

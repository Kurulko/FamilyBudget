using Budget.Initializer;
using Budget.Models.Database;
using Budget.Services.Account;
using Budget.Services.Db;
using Budget.Services.Db.Categories;
using Budget.Services.Db.Operations;
using Budget.Services.Db.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;

IServiceCollection services = builder.Services;

string connection = config.GetConnectionString("DefaultConnection");
services.AddDbContext<BudgetContext>(opts => opts.UseSqlServer(connection));

services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<BudgetContext>();
services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => options.LoginPath = new PathString("/register"));

services.AddScoped<IAccountService, AccountService>();
services.AddScoped<UserService, UserManager>();
services.AddScoped<OperationService, OperationManager>();
services.AddScoped<CategoryService, CategoryManager>();
services.AddScoped<Service<IdentityRole, string>, UserRoleManager>();
services.AddScoped<Service<Currency, long>, CurrencyManager>();
services.AddScoped<DbModelService<Money>, MoneyManager>();

services.AddControllersWithViews().AddNewtonsoftJson();
services.AddSwaggerGen();
services.AddRazorPages();


WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

using (IServiceScope serviceScope = app.Services.CreateScope())
{
    IServiceProvider serviceProvider = serviceScope.ServiceProvider;

    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await RoleInitializer.InitializeAsync(roleManager);

    string adminName = config.GetValue<string>("Admin:Name");
    string adminPassword = config.GetValue<string>("Admin:Password");
    var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
    string? userId = await UsersInitializer.AdminInitializeAsync(userManager, adminName, adminPassword);

    if(userId is not null)
    {
        var operationService = serviceProvider.GetRequiredService<OperationService>();
        var categoryService = serviceProvider.GetRequiredService<CategoryService>();
        var moneyService = serviceProvider.GetRequiredService<DbModelService<Money>>();
        await DataInitializer.InitializeAsync(userId, operationService, categoryService, moneyService);
    }
}

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());

app.Run();


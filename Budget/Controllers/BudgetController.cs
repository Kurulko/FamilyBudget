using Budget.Models;
using Budget.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Controllers;

[Authorize]
public abstract class BudgetController : MainController
{
    protected readonly IUserService userService;
    public BudgetController(IUserService userService)
        => this.userService = userService;
}

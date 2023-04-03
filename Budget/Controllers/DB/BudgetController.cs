using Budget.Models;
using Budget.Services.Db.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Controllers.DB;

[Authorize]
public abstract class BudgetController : MainController
{
    protected readonly AbsUserService userService;
    public BudgetController(AbsUserService userService)
        => this.userService = userService;
}

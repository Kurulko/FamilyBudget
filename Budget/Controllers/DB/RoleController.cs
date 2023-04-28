using Budget.Services.Db.Users;
using Budget.Services.Db;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Budget.Models.Database;
using Budget.Models.ViewModel.Account;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Identity;

namespace Budget.Controllers.DB;

[Route(name)]
public class RoleController : ActiveEditController<IdentityRole, string>
{
    public RoleController(UserService userService, Service<IdentityRole, string> roleService) : base(userService, roleService) { }

    internal const string name = "roles";

    protected override IActionResult RedirectToBack()
        => Redirect($"/{name}/{pathToModels}");
}


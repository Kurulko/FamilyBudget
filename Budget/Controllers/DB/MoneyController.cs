using Budget.Services.Db.Users;
using Budget.Services.Db;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Budget.Models.Database;

namespace Budget.Controllers.DB;

[Route("money")]
public class MoneyController : DbModelEditController<Money>
{
    public MoneyController(AbsUserService userService, DbModelService<Money> dbService) : base(userService, dbService) { }

    protected override IActionResult RedirectToBack()
        => Redirect("/money/models");
}


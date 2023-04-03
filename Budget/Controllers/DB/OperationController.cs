using Budget.Models.Database;
using Budget.Services.Db.Users;
using Budget.Services.Db;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;

namespace Budget.Controllers.DB;

[Route("operations")]
public class OperationController : DbModelEditController<Operation>
{
    public OperationController(AbsUserService userService, DbModelService<Operation> dbService) : base(userService, dbService) { }
}

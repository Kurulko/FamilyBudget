using Microsoft.AspNetCore.Mvc;

namespace Budget.Controllers;

public abstract class MainController : Controller
{
    protected IActionResult RedirectToHome()
        => RedirectToAction("Index", "Home");
}

using Budget.Models;
using Budget.Models.ViewModel.Account;
using Budget.Services.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Budget.Controllers;

public class AccountController : MainController
{
    readonly IAccountService accountService;
    public AccountController(IAccountService accountService)
        => this.accountService = accountService;

    [HttpGet("register")]
    public IActionResult Register() => View();

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            IdentityResult result = await accountService.RegisterAsync(model);

            if (result.Succeeded)
                return RedirectToHome();
            else
                foreach (IdentityError error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
        }
        return View(model);
    }


    [HttpGet("login")]
    public IActionResult Login([FromQuery] string? returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel model, string? returnUrl)
    {
        if (ModelState.IsValid)
        {
            bool result = await accountService.LoginAsync(model);
            if (result)
                return returnUrl is null ? RedirectToHome() : Redirect(returnUrl);

            ModelState.AddModelError(string.Empty, "Wrong password or/and email");
            ViewBag.ReturnUrl = returnUrl;
        }
        return View(model);
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await accountService.LogoutAsync();
        return RedirectToAction(nameof(Login));
    }
}

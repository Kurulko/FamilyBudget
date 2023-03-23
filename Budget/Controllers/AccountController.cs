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


    [HttpGet]
    public IActionResult Register() => View();
    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if(ModelState.IsValid)
        {
            IdentityResult result = await accountService.RegisterAsync(model);

            if(result.Succeeded)
                return RedirectToHome();
            else
                foreach (IdentityError error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
        }
        return View(model);
    }


    [HttpGet]
    public IActionResult Login() => View();
    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if(ModelState.IsValid)
        {
            bool result = await accountService.LoginAsync(model);

            if (result)
                return RedirectToHome();
            else
                ModelState.AddModelError(string.Empty, "Wrong password or/and email");
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await accountService.LogoutAsync();
        return RedirectToAction(nameof(Login));
    }
}

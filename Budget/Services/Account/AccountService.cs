using Budget.Models.Database;
using Budget.Models.ViewModel.Account;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Budget.Services.Account;

public class AccountService : IAccountService
{
    readonly UserManager<User> userManager;
    readonly SignInManager<User> signInManager;

    public AccountService(UserManager<User> userManager, SignInManager<User> signInManager)
        => (this.userManager, this.signInManager) = (userManager, signInManager);

    public async Task<bool> LoginAsync(LoginModel model)
    {
        User user = (User)model;

        var result = await signInManager.PasswordSignInAsync(user,
            model.Password, model.RememberMe, false);

        return result.Succeeded;
    }

    public async Task LogoutAsync()
        => await signInManager.SignOutAsync();

    public async Task<IdentityResult> RegisterAsync(RegisterModel model)
    {
        User user = (User)model;
        IdentityResult result = await userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
            await signInManager.SignInAsync(user, model.IsRememberMe);

        return result;
    }
}

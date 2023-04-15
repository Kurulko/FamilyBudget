using Budget.Models.Database;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Threading.Tasks;
using System;
using Budget.Models.ViewModel.Account;

namespace Budget.Initializer;

public class UsersInitializer
{
    public static async Task<string?> AdminInitializeAsync(UserManager<User> userManager, string name, string password)
    {
        if (await userManager.FindByNameAsync(name) is null)
        {
            User user = new() { UserName = name };
            IdentityResult result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user: user, role: Roles.Admin);
                return user.Id;
            }
        }
        return default;
    }
}

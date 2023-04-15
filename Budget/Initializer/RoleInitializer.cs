using Budget.Models.ViewModel.Account;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Budget.Initializer;

public class RoleInitializer
{
    public static async Task InitializeAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = { Roles.Admin, Roles.User };
        foreach (string roleName in roles)
        {
            IdentityRole? roleDb = await roleManager.FindByIdAsync(roleName);
            if(roleDb is null) 
                await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

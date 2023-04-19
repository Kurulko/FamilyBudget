using Budget.Models;
using Budget.Models.ViewModel.Account;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Budget.Services.Account;

public interface IAccountService
{
    Task<IdentityResult> RegisterAsync(RegisterModel model);
    Task<bool> LoginAsync(LoginModel model);
    Task LogoutAsync();
}

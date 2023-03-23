using Budget.Models.ViewModel.Account;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Budget.Models.Database;

public class User : IdentityUser
{
    public IEnumerable<Money>? CurrentMoney { get; set; }
    public IEnumerable<Operation>? Purchases { get; set; }
    public IEnumerable<Operation>? Salaries { get; set; }


    public static explicit operator User(RegisterModel model)
    {
        User user = new();

        user.Email = model.Email;
        user.UserName = model.Name;

        return user;
    }

    public static explicit operator User(LoginModel model)
    {
        User user = new();

        user.UserName = model.Name;

        return user;
    }
}

using Budget.Models.ViewModel.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Budget.Models.Database;

public class User : IdentityUser
{
    public DateTime RegisteredTime { get; set; } = DateTime.Now;

    public IEnumerable<Money>? Money { get; set; }
    public IEnumerable<Operation>? Operations { get; set; }
    public IEnumerable<Currency>? Currencies { get; set; }
    public IEnumerable<Category>? Categories { get; set; }

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

using System.Collections.Generic;

namespace Budget.Models.ViewModel.Account;

public class RolesAndUserId
{
    public IEnumerable<string> Roles { get; set; } = null!;
    public string? UserId { get; set; }
}

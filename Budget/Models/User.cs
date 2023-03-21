using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Budget.Models;

public class User : IdentityUser
{
    public decimal NowMoneyInCart { get; set; }
    public decimal NowMoneyInCash { get; set; }
    public decimal SpendCash { get; set; }
    public decimal SpendMoneyFromTheCard { get; set; }

    public MoneyForEveryone MoneyForEveryone { get; set; }
    public List<SpendMoney> Spend { get; set; }
    public List<GetMoney> Get { get; set; }
}

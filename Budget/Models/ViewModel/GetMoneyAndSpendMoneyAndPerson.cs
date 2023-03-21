using System.Collections.Generic;

namespace Budget.Models.ViewModel;

public class GetMoneyAndSpendMoneyAndPerson
{
    public string PersonId { get; set; }
    public decimal NowMoneyInCart { get; set; }
    public decimal NowMoneyInCash { get; set; }
    public List<GetMoney> Get { get; set; }
    public List<SpendMoney> Spend { get; set; }
}

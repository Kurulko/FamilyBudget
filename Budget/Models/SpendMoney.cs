using System.ComponentModel.DataAnnotations.Schema;

namespace Budget.Models;

[Table("SpendMoney")]
public class SpendMoney : OperationsWithMoney
{
    public string WasMoney { get; set; }
}

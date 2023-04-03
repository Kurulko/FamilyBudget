namespace Budget.Models.Database;

public class Money : DbModel
{
    public decimal Count { get; set; }
    public bool IsCard { get; set; }
    public bool IsCash { get; set; }

    public long CurrencyId { get; set; }
    public Currency? Currency { get; set; }
    public long OperationId { get; set; }
    public Operation? Operation { get; set; }
}

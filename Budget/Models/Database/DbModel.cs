namespace Budget.Models.Database;

public class DbModel
{
    public long Id { get; set; }

    public string? UserId { get; set; }
    public User? User { get; set; }
}

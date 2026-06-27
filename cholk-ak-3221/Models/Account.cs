namespace cholk_ak_3221.Models;

public enum AccountType { Bank, Trading, Cash }
public enum AccountStatus { Active, Challenge, Inactive }

public class Account
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public string Abbreviation { get; set; } = "";
    public string Subtitle { get; set; } = "";
    public AccountType Type { get; set; }
    public AccountStatus Status { get; set; } = AccountStatus.Active;
    public decimal Balance { get; set; }
    public decimal MonthPnl { get; set; }
    public List<decimal> EquityHistory { get; set; } = new();
}

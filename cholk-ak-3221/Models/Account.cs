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
    public List<AccountOperation> Operations { get; set; } = new();
}

public class AccountOperation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public decimal Amount { get; set; }          // positive = income, negative = expense
    public string Description { get; set; } = "";
    public DateTime Date { get; set; } = DateTime.Today;
}

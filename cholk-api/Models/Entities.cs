namespace cholk_api.Models;

// ── Enums (mirror of Blazor Models) ──────────────────────────────────────────

public enum AccountType { Bank, Trading, Cash }
public enum AccountStatus { Active, Challenge, Inactive }
public enum TradeDirection { Long, Short }
public enum TradeStatus { Open, Closed }
public enum StrategyStatus { Profitable, UnderReview, Inactive }

// ── EF Core entities ─────────────────────────────────────────────────────────

public class AppUser
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<Account> Accounts { get; set; } = [];
    public List<Trade> Trades { get; set; } = [];
    public List<Strategy> Strategies { get; set; } = [];
}

public class Account
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }

    public string Name { get; set; } = "";
    public string Abbreviation { get; set; } = "";
    public string Subtitle { get; set; } = "";
    public AccountType Type { get; set; }
    public AccountStatus Status { get; set; } = AccountStatus.Active;
    public decimal Balance { get; set; }
    public decimal MonthPnl { get; set; }

    // stored as JSON string in SQLite
    public string EquityHistoryJson { get; set; } = "[]";
    public string OperationsJson { get; set; } = "[]";
}

public class Trade
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }

    public string Symbol { get; set; } = "";
    public TradeDirection Direction { get; set; }
    public string AccountName { get; set; } = "";
    public decimal EntryPrice { get; set; }
    public decimal? ExitPrice { get; set; }
    public decimal StopLoss { get; set; }
    public decimal TakeProfit { get; set; }
    public string Setup { get; set; } = "";
    public decimal Pnl { get; set; }
    public decimal PnlPercent { get; set; }
    public DateTime OpenedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public TradeStatus Status { get; set; }
    public string? Notes { get; set; }
}

public class Strategy
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }

    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public StrategyStatus Status { get; set; }
    public decimal TotalPnl { get; set; }
    public decimal WinRate { get; set; }
    public decimal ProfitFactor { get; set; }
    public int TradeCount { get; set; }

    public string EquityHistoryJson { get; set; } = "[]";
}

// ── DTOs ─────────────────────────────────────────────────────────────────────

public record RegisterRequest(string Email, string Password, string DisplayName);
public record LoginRequest(string Email, string Password);
public record AuthResponse(string Token, string Email, string DisplayName);

namespace cholk_ak_3221.Models;

public enum StrategyStatus { Profitable, UnderReview, Inactive }

public class Strategy
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal TotalPnl { get; set; }
    public double WinRate { get; set; }
    public double ProfitFactor { get; set; }
    public int TradeCount { get; set; }
    public StrategyStatus Status { get; set; } = StrategyStatus.Profitable;
    public List<decimal> EquityHistory { get; set; } = new();
}

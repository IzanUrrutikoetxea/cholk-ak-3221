namespace cholk_ak_3221.Models;

public enum TradeDirection { Long, Short }
public enum TradeStatus { Open, Closed }

public class Trade
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Symbol { get; set; } = "";
    public TradeDirection Direction { get; set; }
    public Guid AccountId { get; set; }
    public string AccountName { get; set; } = "";
    public decimal EntryPrice { get; set; }
    public decimal? ExitPrice { get; set; }
    public decimal StopLoss { get; set; }
    public decimal TakeProfit { get; set; }
    public string Setup { get; set; } = "";
    public decimal Pnl { get; set; }
    public decimal PnlPercent { get; set; }
    public DateTime OpenedAt { get; set; } = DateTime.Today;
    public DateTime? ClosedAt { get; set; }
    public TradeStatus Status { get; set; } = TradeStatus.Closed;
    public string? Notes { get; set; }
}

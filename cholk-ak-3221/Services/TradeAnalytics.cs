using cholk_ak_3221.Models;

namespace cholk_ak_3221.Services;

public static class TradeAnalytics
{
    public static double WinRate(IEnumerable<Trade> trades)
    {
        var closed = trades.Where(t => t.Status == TradeStatus.Closed).ToList();
        if (closed.Count == 0) return 0;
        return (double)closed.Count(t => t.Pnl > 0) / closed.Count * 100;
    }

    public static double ProfitFactor(IEnumerable<Trade> trades)
    {
        var grossProfit = (double)trades.Where(t => t.Pnl > 0).Sum(t => t.Pnl);
        var grossLoss = Math.Abs((double)trades.Where(t => t.Pnl < 0).Sum(t => t.Pnl));
        return grossLoss == 0 ? 0 : grossProfit / grossLoss;
    }

    public static Dictionary<string, decimal> PnlBySymbol(IEnumerable<Trade> trades)
        => trades.GroupBy(t => t.Symbol)
                 .OrderByDescending(g => g.Sum(t => t.Pnl))
                 .ToDictionary(g => g.Key, g => g.Sum(t => t.Pnl));

    public static Dictionary<DayOfWeek, decimal> PnlByDayOfWeek(IEnumerable<Trade> trades)
        => trades.Where(t => t.ClosedAt.HasValue)
                 .GroupBy(t => t.ClosedAt!.Value.DayOfWeek)
                 .ToDictionary(g => g.Key, g => g.Sum(t => t.Pnl));

    public static Dictionary<int, decimal> PnlByMonth(IEnumerable<Trade> trades)
        => trades.Where(t => t.ClosedAt.HasValue)
                 .GroupBy(t => t.ClosedAt!.Value.Month)
                 .OrderBy(g => g.Key)
                 .ToDictionary(g => g.Key, g => g.Sum(t => t.Pnl));

    public static List<decimal> EquityCurve(IEnumerable<Trade> trades, decimal startingCapital)
    {
        var sorted = trades.Where(t => t.ClosedAt.HasValue)
                           .OrderBy(t => t.ClosedAt)
                           .ToList();
        var curve = new List<decimal> { startingCapital };
        var running = startingCapital;
        foreach (var t in sorted)
        {
            running += t.Pnl;
            curve.Add(running);
        }
        return curve;
    }

    public static int MaxWinStreak(IEnumerable<Trade> trades)
    {
        var sorted = trades.Where(t => t.Status == TradeStatus.Closed)
                           .OrderBy(t => t.ClosedAt).ToList();
        int max = 0, current = 0;
        foreach (var t in sorted)
        {
            if (t.Pnl > 0) { current++; max = Math.Max(max, current); }
            else current = 0;
        }
        return max;
    }
}

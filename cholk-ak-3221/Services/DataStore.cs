using Blazored.LocalStorage;
using cholk_ak_3221.Models;

namespace cholk_ak_3221.Services;

public class DataStore(ILocalStorageService storage) : IDataStore
{
    private const string AccountsKey = "cholk_accounts";
    private const string TradesKey = "cholk_trades";
    private const string StrategiesKey = "cholk_strategies";

    // ── Accounts ──────────────────────────────────────────────────────────

    public async Task<List<Account>> GetAccountsAsync()
        => await storage.GetItemAsync<List<Account>>(AccountsKey) ?? SeedAccounts();

    public async Task SaveAccountsAsync(List<Account> accounts)
        => await storage.SetItemAsync(AccountsKey, accounts);

    public async Task AddAccountAsync(Account account)
    {
        var list = await GetAccountsAsync();
        list.Add(account);
        await SaveAccountsAsync(list);
    }

    public async Task UpdateAccountAsync(Account account)
    {
        var list = await GetAccountsAsync();
        var idx = list.FindIndex(a => a.Id == account.Id);
        if (idx >= 0) list[idx] = account;
        await SaveAccountsAsync(list);
    }

    public async Task DeleteAccountAsync(Guid id)
    {
        var list = await GetAccountsAsync();
        list.RemoveAll(a => a.Id == id);
        await SaveAccountsAsync(list);
    }

    // ── Trades ────────────────────────────────────────────────────────────

    public async Task<List<Trade>> GetTradesAsync()
        => await storage.GetItemAsync<List<Trade>>(TradesKey) ?? SeedTrades();

    public async Task SaveTradesAsync(List<Trade> trades)
        => await storage.SetItemAsync(TradesKey, trades);

    public async Task AddTradeAsync(Trade trade)
    {
        var list = await GetTradesAsync();
        list.Add(trade);
        await SaveTradesAsync(list);
    }

    public async Task UpdateTradeAsync(Trade trade)
    {
        var list = await GetTradesAsync();
        var idx = list.FindIndex(t => t.Id == trade.Id);
        if (idx >= 0) list[idx] = trade;
        await SaveTradesAsync(list);
    }

    public async Task DeleteTradeAsync(Guid id)
    {
        var list = await GetTradesAsync();
        list.RemoveAll(t => t.Id == id);
        await SaveTradesAsync(list);
    }

    // ── Strategies ────────────────────────────────────────────────────────

    public async Task<List<Strategy>> GetStrategiesAsync()
        => await storage.GetItemAsync<List<Strategy>>(StrategiesKey) ?? SeedStrategies();

    public async Task SaveStrategiesAsync(List<Strategy> strategies)
        => await storage.SetItemAsync(StrategiesKey, strategies);

    public async Task AddStrategyAsync(Strategy strategy)
    {
        var list = await GetStrategiesAsync();
        list.Add(strategy);
        await SaveStrategiesAsync(list);
    }

    public async Task UpdateStrategyAsync(Strategy strategy)
    {
        var list = await GetStrategiesAsync();
        var idx = list.FindIndex(s => s.Id == strategy.Id);
        if (idx >= 0) list[idx] = strategy;
        await SaveStrategiesAsync(list);
    }

    public async Task DeleteStrategyAsync(Guid id)
    {
        var list = await GetStrategiesAsync();
        list.RemoveAll(s => s.Id == id);
        await SaveStrategiesAsync(list);
    }

    // ── Seed data ─────────────────────────────────────────────────────────

    private static List<Account> SeedAccounts() =>
    [
        new() { Name = "CaixaBank", Abbreviation = "CX", Subtitle = "Cuenta corriente", Type = AccountType.Bank,
            Balance = 42300, MonthPnl = 85,
            EquityHistory = [40000, 40500, 41000, 40800, 41200, 41800, 42100, 42300] },
        new() { Name = "Efectivo", Abbreviation = "€", Subtitle = "Liquidez", Type = AccountType.Cash,
            Balance = 3150, MonthPnl = 0,
            EquityHistory = [3150, 3150, 3150, 3150, 3150, 3150, 3150, 3150] },
        new() { Name = "Pepperstone", Abbreviation = "PP", Subtitle = "Razor · personal", Type = AccountType.Trading,
            Status = AccountStatus.Active, Balance = 39500, MonthPnl = 1180,
            EquityHistory = [36000, 36500, 37000, 36800, 37400, 38000, 38800, 39500] },
        new() { Name = "FTMO Challenge", Abbreviation = "FT", Subtitle = "Fondeo · 100k", Type = AccountType.Trading,
            Status = AccountStatus.Challenge, Balance = 25000, MonthPnl = 590,
            EquityHistory = [23500, 23800, 24000, 23900, 24200, 24500, 24800, 25000] },
        new() { Name = "IC Markets", Abbreviation = "IC", Subtitle = "Raw · personal", Type = AccountType.Trading,
            Status = AccountStatus.Active, Balance = 18500, MonthPnl = 1060,
            EquityHistory = [16500, 17000, 17200, 17100, 17600, 18000, 18200, 18500] },
    ];

    private static List<Trade> SeedTrades()
    {
        var ppId = Guid.NewGuid();
        var ftId = Guid.NewGuid();
        var icId = Guid.NewGuid();
        var base_date = new DateTime(2026, 6, 1);
        return
        [
            new() { Symbol = "EURUSD", Direction = TradeDirection.Long, AccountName = "Pepperstone",
                EntryPrice = 1.0842m, ExitPrice = 1.0905m, StopLoss = 1.0810m, TakeProfit = 1.0920m,
                Setup = "Breakout London", Pnl = 420, PnlPercent = 1.2m,
                OpenedAt = base_date.AddDays(22), ClosedAt = base_date.AddDays(22), Status = TradeStatus.Closed },
            new() { Symbol = "GBPUSD", Direction = TradeDirection.Short, AccountName = "IC Markets",
                EntryPrice = 1.2710m, ExitPrice = 1.2748m, StopLoss = 1.2760m, TakeProfit = 1.2660m,
                Setup = "Reversal NY", Pnl = -180, PnlPercent = -0.8m,
                OpenedAt = base_date.AddDays(22), ClosedAt = base_date.AddDays(22), Status = TradeStatus.Closed },
            new() { Symbol = "USDJPY", Direction = TradeDirection.Long, AccountName = "FTMO",
                EntryPrice = 156.20m, ExitPrice = 157.85m, StopLoss = 155.60m, TakeProfit = 158.20m,
                Setup = "Trend H4", Pnl = 640, PnlPercent = 2.1m,
                OpenedAt = base_date.AddDays(21), ClosedAt = base_date.AddDays(21), Status = TradeStatus.Closed },
            new() { Symbol = "EURJPY", Direction = TradeDirection.Long, AccountName = "Pepperstone",
                EntryPrice = 169.40m, ExitPrice = 170.10m, StopLoss = 168.90m, TakeProfit = 170.50m,
                Setup = "Trend H4", Pnl = 95, PnlPercent = 0.4m,
                OpenedAt = base_date.AddDays(21), ClosedAt = base_date.AddDays(21), Status = TradeStatus.Closed },
            new() { Symbol = "AUDUSD", Direction = TradeDirection.Long, AccountName = "Pepperstone",
                EntryPrice = 0.6655m, ExitPrice = 0.6612m, StopLoss = 0.6630m, TakeProfit = 0.6710m,
                Setup = "Pullback Asia", Pnl = -210, PnlPercent = -1.0m,
                OpenedAt = base_date.AddDays(20), ClosedAt = base_date.AddDays(20), Status = TradeStatus.Closed },
            new() { Symbol = "USDCAD", Direction = TradeDirection.Short, AccountName = "IC Markets",
                EntryPrice = 1.3720m, ExitPrice = 1.3665m, StopLoss = 1.3750m, TakeProfit = 1.3640m,
                Setup = "Breakout London", Pnl = 380, PnlPercent = 1.5m,
                OpenedAt = base_date.AddDays(20), ClosedAt = base_date.AddDays(20), Status = TradeStatus.Closed },
            new() { Symbol = "EURGBP", Direction = TradeDirection.Long, AccountName = "FTMO",
                EntryPrice = 0.8520m, ExitPrice = 0.8548m, StopLoss = 0.8500m, TakeProfit = 0.8560m,
                Setup = "Trend H4", Pnl = 260, PnlPercent = 0.9m,
                OpenedAt = base_date.AddDays(19), ClosedAt = base_date.AddDays(19), Status = TradeStatus.Closed },
            new() { Symbol = "NZDUSD", Direction = TradeDirection.Short, AccountName = "Pepperstone",
                EntryPrice = 0.6090m, ExitPrice = 0.6118m, StopLoss = 0.6125m, TakeProfit = 0.6040m,
                Setup = "Reversal NY", Pnl = -150, PnlPercent = -0.7m,
                OpenedAt = base_date.AddDays(19), ClosedAt = base_date.AddDays(19), Status = TradeStatus.Closed },
            new() { Symbol = "GBPJPY", Direction = TradeDirection.Long, AccountName = "IC Markets",
                EntryPrice = 198.40m, ExitPrice = 200.10m, StopLoss = 197.60m, TakeProfit = 201.00m,
                Setup = "Breakout London", Pnl = 510, PnlPercent = 1.8m,
                OpenedAt = base_date.AddDays(18), ClosedAt = base_date.AddDays(18), Status = TradeStatus.Closed },
            new() { Symbol = "USDCHF", Direction = TradeDirection.Short, AccountName = "FTMO",
                EntryPrice = 0.8950m, ExitPrice = 0.8915m, StopLoss = 0.8975m, TakeProfit = 0.8900m,
                Setup = "Pullback Asia", Pnl = 290, PnlPercent = 1.1m,
                OpenedAt = base_date.AddDays(18), ClosedAt = base_date.AddDays(18), Status = TradeStatus.Closed },
            new() { Symbol = "EURUSD", Direction = TradeDirection.Long, AccountName = "Pepperstone",
                EntryPrice = 1.0800m, ExitPrice = 1.0850m, StopLoss = 1.0770m, TakeProfit = 1.0870m,
                Setup = "Breakout London", Pnl = 350, PnlPercent = 1.1m,
                OpenedAt = base_date.AddDays(15), ClosedAt = base_date.AddDays(15), Status = TradeStatus.Closed },
            new() { Symbol = "USDJPY", Direction = TradeDirection.Short, AccountName = "IC Markets",
                EntryPrice = 157.50m, ExitPrice = 156.80m, StopLoss = 158.00m, TakeProfit = 156.00m,
                Setup = "Reversal NY", Pnl = -120, PnlPercent = -0.5m,
                OpenedAt = base_date.AddDays(14), ClosedAt = base_date.AddDays(14), Status = TradeStatus.Closed },
            new() { Symbol = "GBPUSD", Direction = TradeDirection.Long, AccountName = "FTMO",
                EntryPrice = 1.2680m, ExitPrice = 1.2750m, StopLoss = 1.2640m, TakeProfit = 1.2780m,
                Setup = "Trend H4", Pnl = 480, PnlPercent = 1.9m,
                OpenedAt = base_date.AddDays(12), ClosedAt = base_date.AddDays(12), Status = TradeStatus.Closed },
            new() { Symbol = "AUDUSD", Direction = TradeDirection.Short, AccountName = "Pepperstone",
                EntryPrice = 0.6700m, ExitPrice = 0.6650m, StopLoss = 0.6730m, TakeProfit = 0.6630m,
                Setup = "Pullback Asia", Pnl = 220, PnlPercent = 1.0m,
                OpenedAt = base_date.AddDays(10), ClosedAt = base_date.AddDays(10), Status = TradeStatus.Closed },
            // Open trades
            new() { Symbol = "EURUSD", Direction = TradeDirection.Long, AccountName = "Pepperstone",
                EntryPrice = 1.0860m, StopLoss = 1.0830m, TakeProfit = 1.0920m,
                Setup = "Breakout London", Pnl = 0, PnlPercent = 0,
                OpenedAt = base_date.AddDays(23), Status = TradeStatus.Open },
            new() { Symbol = "USDJPY", Direction = TradeDirection.Long, AccountName = "FTMO",
                EntryPrice = 157.20m, StopLoss = 156.60m, TakeProfit = 158.80m,
                Setup = "Trend H4", Pnl = 0, PnlPercent = 0,
                OpenedAt = base_date.AddDays(23), Status = TradeStatus.Open },
            new() { Symbol = "GBPJPY", Direction = TradeDirection.Short, AccountName = "IC Markets",
                EntryPrice = 200.50m, StopLoss = 201.20m, TakeProfit = 198.80m,
                Setup = "Reversal NY", Pnl = 0, PnlPercent = 0,
                OpenedAt = base_date.AddDays(23), Status = TradeStatus.Open },
        ];
    }

    private static List<Strategy> SeedStrategies() =>
    [
        new() { Name = "Breakout London", Description = "Roturas de rango en apertura Londres",
            Status = StrategyStatus.Profitable, TotalPnl = 1150, WinRate = 0.65, ProfitFactor = 2.1,
            TradeCount = 20, EquityHistory = [1000, 1080, 1150, 1100, 1200, 1180, 1220, 1150] },
        new() { Name = "Trend H4", Description = "Seguimiento de tendencia en timeframe H4",
            Status = StrategyStatus.Profitable, TotalPnl = 1475, WinRate = 0.70, ProfitFactor = 2.4,
            TradeCount = 17, EquityHistory = [1000, 1100, 1200, 1180, 1300, 1350, 1420, 1475] },
        new() { Name = "Reversal NY", Description = "Reversiones en apertura Nueva York",
            Status = StrategyStatus.UnderReview, TotalPnl = -300, WinRate = 0.40, ProfitFactor = 0.8,
            TradeCount = 10, EquityHistory = [1000, 950, 900, 920, 880, 860, 820, 700] },
        new() { Name = "Pullback Asia", Description = "Pullbacks en sesión Asia",
            Status = StrategyStatus.Profitable, TotalPnl = 500, WinRate = 0.60, ProfitFactor = 1.8,
            TradeCount = 8, EquityHistory = [1000, 1050, 1030, 1080, 1060, 1100, 1090, 1120] },
    ];
}

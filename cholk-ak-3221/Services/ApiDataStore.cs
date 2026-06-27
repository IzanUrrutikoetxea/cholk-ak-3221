using System.Net.Http.Json;
using cholk_ak_3221.Models;

namespace cholk_ak_3221.Services;

/// <summary>
/// IDataStore implementation that persists data in the backend API.
/// Each call requires a valid JWT (set by AuthService on login).
/// </summary>
public class ApiDataStore(HttpClient http) : IDataStore
{
    // ── Accounts ──────────────────────────────────────────────────────────────

    public async Task<List<Account>> GetAccountsAsync()
        => await http.GetFromJsonAsync<List<Account>>("/api/accounts") ?? [];

    public async Task SaveAccountsAsync(List<Account> accounts)
    {
        foreach (var a in accounts)
            await UpdateAccountAsync(a);
    }

    public async Task AddAccountAsync(Account account)
        => await http.PostAsJsonAsync("/api/accounts", account);

    public async Task UpdateAccountAsync(Account account)
        => await http.PutAsJsonAsync($"/api/accounts/{account.Id}", account);

    public async Task DeleteAccountAsync(Guid id)
        => await http.DeleteAsync($"/api/accounts/{id}");

    // ── Trades ────────────────────────────────────────────────────────────────

    public async Task<List<Trade>> GetTradesAsync()
        => await http.GetFromJsonAsync<List<Trade>>("/api/trades") ?? [];

    public async Task SaveTradesAsync(List<Trade> trades)
    {
        foreach (var t in trades)
            await UpdateTradeAsync(t);
    }

    public async Task AddTradeAsync(Trade trade)
        => await http.PostAsJsonAsync("/api/trades", trade);

    public async Task UpdateTradeAsync(Trade trade)
        => await http.PutAsJsonAsync($"/api/trades/{trade.Id}", trade);

    public async Task DeleteTradeAsync(Guid id)
        => await http.DeleteAsync($"/api/trades/{id}");

    // ── Strategies ────────────────────────────────────────────────────────────

    public async Task<List<Strategy>> GetStrategiesAsync()
        => await http.GetFromJsonAsync<List<Strategy>>("/api/strategies") ?? [];

    public async Task SaveStrategiesAsync(List<Strategy> strategies)
    {
        foreach (var s in strategies)
            await UpdateStrategyAsync(s);
    }

    public async Task AddStrategyAsync(Strategy strategy)
        => await http.PostAsJsonAsync("/api/strategies", strategy);

    public async Task UpdateStrategyAsync(Strategy strategy)
        => await http.PutAsJsonAsync($"/api/strategies/{strategy.Id}", strategy);

    public async Task DeleteStrategyAsync(Guid id)
        => await http.DeleteAsync($"/api/strategies/{id}");
}

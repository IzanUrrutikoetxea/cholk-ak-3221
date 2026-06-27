using cholk_ak_3221.Models;

namespace cholk_ak_3221.Services;

public interface IDataStore
{
    // Accounts
    Task<List<Account>> GetAccountsAsync();
    Task SaveAccountsAsync(List<Account> accounts);
    Task AddAccountAsync(Account account);
    Task UpdateAccountAsync(Account account);
    Task DeleteAccountAsync(Guid id);

    // Trades
    Task<List<Trade>> GetTradesAsync();
    Task SaveTradesAsync(List<Trade> trades);
    Task AddTradeAsync(Trade trade);
    Task UpdateTradeAsync(Trade trade);
    Task DeleteTradeAsync(Guid id);

    // Strategies
    Task<List<Strategy>> GetStrategiesAsync();
    Task SaveStrategiesAsync(List<Strategy> strategies);
    Task AddStrategyAsync(Strategy strategy);
    Task UpdateStrategyAsync(Strategy strategy);
    Task DeleteStrategyAsync(Guid id);
}

using cholk_api.Models;
using Microsoft.EntityFrameworkCore;

namespace cholk_api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Trade> Trades => Set<Trade>();
    public DbSet<Strategy> Strategies => Set<Strategy>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<AppUser>(e =>
        {
            e.HasKey(u => u.Id);
            e.HasIndex(u => u.Email).IsUnique();
        });

        b.Entity<Account>(e =>
        {
            e.HasKey(a => a.Id);
            e.Property(a => a.Balance).HasColumnType("TEXT");
            e.Property(a => a.MonthPnl).HasColumnType("TEXT");
        });

        b.Entity<Trade>(e =>
        {
            e.HasKey(t => t.Id);
            e.Property(t => t.EntryPrice).HasColumnType("TEXT");
            e.Property(t => t.ExitPrice).HasColumnType("TEXT");
            e.Property(t => t.StopLoss).HasColumnType("TEXT");
            e.Property(t => t.TakeProfit).HasColumnType("TEXT");
            e.Property(t => t.Pnl).HasColumnType("TEXT");
            e.Property(t => t.PnlPercent).HasColumnType("TEXT");
        });

        b.Entity<Strategy>(e =>
        {
            e.HasKey(s => s.Id);
            e.Property(s => s.TotalPnl).HasColumnType("TEXT");
            e.Property(s => s.WinRate).HasColumnType("TEXT");
            e.Property(s => s.ProfitFactor).HasColumnType("TEXT");
        });
    }
}
